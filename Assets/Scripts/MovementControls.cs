using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementControls : MonoBehaviour
{
	public float speed = 1000;

	public float forwardAngle;

	private new Rigidbody rigidbody;

	void Start()
	{
		this.rigidbody = GetComponent<Rigidbody>();
		Cursor.lockState = CursorLockMode.Locked;
	}

	void Update()
	{
		var input = GetInputVector();
		var torque = input.Rotate(this.forwardAngle) * this.speed;
		this.rigidbody.AddTorque(torque.x, 0, torque.y);
		this.rigidbody.maxAngularVelocity = 100;
	}

	private Vector2 GetInputVector()
	{
		var velX = Input.GetAxisRaw("Horizontal");
		var velY = Input.GetAxisRaw("Vertical");
		return new Vector2(-velY, velX);
	}
}
