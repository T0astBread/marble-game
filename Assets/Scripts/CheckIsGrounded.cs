using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckIsGrounded : MonoBehaviour
{
	public float maxGroundedDistance = .1f;

	public bool IsGrounded()
	{
		Debug.DrawRay(transform.position, Vector3.down, Color.red, 1);
		return Physics.Raycast(transform.position, Vector3.down, this.maxGroundedDistance);
	}

	public bool IsTouchingSurface()
	{
		var hitColliders = Physics.OverlapSphere(transform.position, this.maxGroundedDistance);
		return hitColliders.Length > 1;
	}
}
