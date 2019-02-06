using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(CheckIsGrounded))]
public class JumpControls : MonoBehaviour
{
	public float jumpForceFactor;

	private new Rigidbody rigidbody;
	private CheckIsGrounded checkIsGrounded;

	void Start()
	{
		this.rigidbody = GetComponent<Rigidbody>();
		this.checkIsGrounded = GetComponent<CheckIsGrounded>();
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space) && this.checkIsGrounded.IsGrounded())
		{
			CancelVelocity();
			AddJumpForce();
		}
	}

	private void CancelVelocity()
	{
		this.rigidbody.velocity = new Vector3(this.rigidbody.velocity.x, 0, this.rigidbody.velocity.z);
	}

	private void AddJumpForce()
	{
		this.rigidbody.AddForce(Vector3.up * this.rigidbody.mass * this.jumpForceFactor);
	}
}
