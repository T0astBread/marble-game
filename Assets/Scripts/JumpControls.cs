using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(CheckIsGrounded))]
public class JumpControls : MonoBehaviour
{
	private const float INPUT_BUFFER_TIME = .25f;

	public float jumpForceFactor;

	private new Rigidbody rigidbody;
	private CheckIsGrounded checkIsGrounded;

	private Dictionary<string, SavedContactPoints> contactPoints = new Dictionary<string, SavedContactPoints>();

	private bool inputIsBuffered;
	private float lastInputTime;


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
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			this.inputIsBuffered = true;
			this.lastInputTime = Time.time;
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

	private void Jump()
	{
		if (!this.contactPoints.Any())
		{
			return;
		}

		FilterContactPoints();

		if (!this.contactPoints.Any())
		{
			return;
		}

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


	struct SavedContactPoints
	{
		public float contactTime { get; set; }
		public ContactPoint[] contactPoints { get; set; }
	}
}
