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

	private ContactPoint[] contactPoints;

	private bool inputIsBuffered;
	private float lastInputTime;


	public Vector3 jumpDirection
	{
		get
		{
			var jumpDir = contactPoints
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
	}

	private void Jump()
	{
		CancelVelocity();
		AddJumpForce();
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
		this.contactPoints = collision.contacts;

		if(this.inputIsBuffered)
		{
			Jump();
		}
	}
}
