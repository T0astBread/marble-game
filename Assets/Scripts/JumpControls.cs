using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(CheckIsGrounded)), RequireComponent(typeof(CheckIsOnMovingPlatform))]
public class JumpControls : MonoBehaviour
{
	private const float INPUT_BUFFER_TIME = .25f, PLATFORM_IMPULSE_BUFFER_TIME = .25f;
	private const float PLATFROM_JUMP_AMP_MIN_PROGRESS = .25f, PLATFORM_JUMP_AMPLIFICATION = 1.5f;

	public float jumpForceFactor;
	public bool jumpIsDisabled;

	private new Rigidbody rigidbody;
	private CheckIsGrounded checkIsGrounded;
	private CheckIsOnMovingPlatform checkIsOnMovingPlatform;

	private Dictionary<string, SavedContactPoints> contactPoints = new Dictionary<string, SavedContactPoints>();

	private bool inputIsBuffered;
	private float lastInputTime;

	private bool movingPlatformImpulseIsBuffered;
	private Vector3 bufferedMovingPlatformImpulse;
	private float lastMovingPlatformImpulseTime;


	public Vector3 jumpDirection
	{
		get
		{
			var jumpDir = contactPoints
				.SelectMany(c => c.Value.contactPoints)
				.Select(c => c.normal)
				.Aggregate((n1, n2) => n1 + n2)
				.normalized;
			Debug.DrawRay(transform.position, jumpDir, Color.magenta, 1);
			return jumpDir;
		}
	}


	void Start()
	{
		this.rigidbody = GetComponent<Rigidbody>();
		this.checkIsGrounded = GetComponent<CheckIsGrounded>();
		this.checkIsOnMovingPlatform = GetComponent<CheckIsOnMovingPlatform>();
	}

	void Update()
	{
		if (this.jumpIsDisabled)
		{
			return;
		}

		if (Input.GetKeyDown(KeyCode.Space))
		{
			if (CanRequestPlatformImpulseAmplification())
			{
				RequestPlatformImpulseAmplification();
			}
			else
			{
				this.inputIsBuffered = true;
				this.lastInputTime = Time.time;
			}
		}

		if (this.bufferedMovingPlatformImpulse != null)
		{
			float timeSinceLastMovingPlatformImpulse = Time.time - this.lastMovingPlatformImpulseTime;
			if (timeSinceLastMovingPlatformImpulse > PLATFORM_IMPULSE_BUFFER_TIME)
			{
				this.movingPlatformImpulseIsBuffered = false;
			}
		}

		float timeSinceLastInput = Time.time - this.lastInputTime;
		if (this.inputIsBuffered && timeSinceLastInput > INPUT_BUFFER_TIME)
		{
			this.inputIsBuffered = false;
		}

		if (this.inputIsBuffered)
		{
			Jump();
		}
	}

	private bool CanRequestPlatformImpulseAmplification()
	{
		return this.checkIsOnMovingPlatform.isOnMovingPlatform &&
			this.checkIsOnMovingPlatform.platform.canAmplifyJump &&
			this.checkIsOnMovingPlatform.platform.currentProgress > PLATFROM_JUMP_AMP_MIN_PROGRESS;
	}

	private void RequestPlatformImpulseAmplification()
	{
		this.checkIsOnMovingPlatform.platform.RequestImpulseAmplification(gameObject, PLATFORM_JUMP_AMPLIFICATION);
	}

	private void Jump()
	{
		if (CanJumpFromMovingPlatformImpulse())
		{
			JumpFromMovingPlatformImpulse();
		}
		else if (CanJumpFromGround())
		{
			JumpFromGround();
		}
	}

	private bool CanJumpFromGround()
	{
		if (!this.contactPoints.Any())
		{
			return false;
		}

		FilterContactPoints();

		return this.contactPoints.Any();
	}

	private void JumpFromGround()
	{

#if UNITY_EDITOR
		Debug.Log("Jumping off of " + contactPoints.Count + " objects: " + string.Join(", ", contactPoints.Select(c => c.Key).ToArray()));
#endif

		CancelVelocity();
		AddJumpForce();

		this.inputIsBuffered = false;

		this.contactPoints.Clear();
	}

	private void FilterContactPoints()
	{
		HashSet<string> touchingGameObjects = GetTouchingGameObjects();
		this.contactPoints = this.contactPoints
			.Where(c => Time.time - c.Value.contactTime <= INPUT_BUFFER_TIME)
			.Where(c => touchingGameObjects.Contains(c.Key))
			.ToDictionary(c => c.Key, c => c.Value);
	}

	private HashSet<string> GetTouchingGameObjects()
	{
		var touchingColliders = Physics.OverlapSphere(transform.position, 2);
		var touchingGameObjects = new HashSet<string>();
		foreach (var collider in touchingColliders)
		{
			touchingGameObjects.Add(collider.gameObject.name);
		}
		return touchingGameObjects;
	}

	private void CancelVelocity()
	{
		this.rigidbody.velocity = new Vector3(this.rigidbody.velocity.x, 0, this.rigidbody.velocity.z);
	}

	private void AddJumpForce()
	{
		this.rigidbody.AddForce(this.jumpDirection * this.rigidbody.mass * this.jumpForceFactor);
	}

	private bool CanJumpFromMovingPlatformImpulse()
	{
		return this.movingPlatformImpulseIsBuffered;
	}

	private void JumpFromMovingPlatformImpulse()
	{
		Debug.Log("Jumping off of moving platform impulse");

		var impulseJump = this.bufferedMovingPlatformImpulse * (PLATFORM_JUMP_AMPLIFICATION - 1);
		Debug.DrawRay(transform.position, impulseJump, Color.green, 1);

		this.rigidbody.AddForce(impulseJump, ForceMode.Impulse);

		this.movingPlatformImpulseIsBuffered = false;
		this.inputIsBuffered = false;
	}

	void OnCollisionEnter(Collision collision)
	{
		OnCollision(collision);
	}

	void OnCollisionStay(Collision collision)
	{
		OnCollision(collision);
	}

	private void OnCollision(Collision collision)
	{
		this.contactPoints[collision.gameObject.name] = new SavedContactPoints
		{
			contactTime = Time.time,
			contactPoints = collision.contacts
		};

		if (this.inputIsBuffered)
		{
			Jump();
		}
	}

	void OnMovingPlatformImpulse(object[] args)
	{
		var platform = args[0] as MovingPlatform;
		Vector3 impulse = (Vector3)args[1];
		float amplification = (float)args[2];

		if (platform.canAmplifyJump && amplification == 1)
		{
			this.movingPlatformImpulseIsBuffered = true;
			this.bufferedMovingPlatformImpulse = impulse;
			this.lastMovingPlatformImpulseTime = Time.time;
		}
	}

	void DisableMovement()
	{
		this.jumpIsDisabled = true;
	}

	void EnableMovement()
	{
		this.jumpIsDisabled = false;
	}


	struct SavedContactPoints
	{
		public float contactTime { get; set; }
		public ContactPoint[] contactPoints { get; set; }
	}
}
