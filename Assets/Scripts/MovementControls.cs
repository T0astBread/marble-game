using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(CheckIsGrounded))]
public class MovementControls : MonoBehaviour
{
	public float rollSpeed = 2000, airSpeed = 4000;

	public float forwardAngle;

	private new Rigidbody rigidbody;
	private CheckIsGrounded checkIsGrounded;

	void Start()
	{
		this.rigidbody = GetComponent<Rigidbody>();
		this.checkIsGrounded = GetComponent<CheckIsGrounded>();
	}

	void FixedUpdate()
	{
#if UNITY_EDITOR
		DebugDrawForwardAngle();
#endif

		var input = GetInputVector();
		var rotatedInput = input.Rotate(this.forwardAngle);

		ApplyRollTorque(rotatedInput);
		if (!this.checkIsGrounded.IsGrounded())
		{
			ApplyAirMovementForce(rotatedInput);
		}

		this.rigidbody.maxAngularVelocity = 100;
	}

	private void DebugDrawForwardAngle()
	{
		var forward = Vector2.up.Rotate(this.forwardAngle);
		Debug.DrawLine(transform.position, transform.position + new Vector3(forward.x, 0, forward.y), Color.red);
	}

	private Vector2 GetInputVector()
	{
		var velX = Input.GetAxisRaw("Horizontal");
		var velY = Input.GetAxisRaw("Vertical");
		return new Vector2(velX, velY);
	}

	private void ApplyRollTorque(Vector2 rotatedInput)
	{
		var torque = -rotatedInput * this.rollSpeed;
		this.rigidbody.AddTorque(-torque.y, 0, torque.x);
	}

	private void ApplyAirMovementForce(Vector2 rotatedInput)
	{
		var force = rotatedInput * this.airSpeed;
		this.rigidbody.AddForce(force.x, 0, force.y);
	}
}
