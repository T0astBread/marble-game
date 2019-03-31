using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ArtificiallyRoll : MonoBehaviour
{
	public float rotationMultiplier = 100;

	private Vector3 prevPos;

	void Update()
	{
		var modelContainer = transform.GetChild(0).GetChild(0);
		var posDiff = (transform.position - prevPos) * this.rotationMultiplier;
		modelContainer.Rotate(posDiff.z, 0, -posDiff.x, Space.World);
		this.prevPos = transform.position;
	}
}
