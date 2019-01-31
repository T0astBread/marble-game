using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementControls : MonoBehaviour
{
	private new Rigidbody rigidbody;

	void Start()
	{
		this.rigidbody = GetComponent<Rigidbody>();
	}

	void Update()
	{
		var input = GetInputVector() * 1000;
		// this.rigidbody.velocity = new Vector3(input.x, this.rigidbody.velocity.y, input.z);
		// this.rigidbody.rotation *= Quaternion.AngleAxis(input.x, Vector3.forward) * Quaternion.AngleAxis(input.y, Vector3.right);
		this.rigidbody.AddTorque(input.z, 0, -input.x);
		this.rigidbody.maxAngularVelocity = 100;
	}

	private Vector3 GetInputVector()
	{
		var velX = Input.GetAxisRaw("Horizontal");
		var velY = Input.GetAxisRaw("Vertical");
		return new Vector3(velX, 0, velY);
	}
}
