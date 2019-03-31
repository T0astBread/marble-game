using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePlayerOnProximity : MonoBehaviour
{
	public float minProximity;
	public float rotationDegreesPerSec;

	private GameObject player;
	private bool isFacingPlayer;
	private RandomDestinationWalk walkBehaviour;

	void Start()
	{
		this.player = GameObject.Find("Marble");
		this.walkBehaviour = GetComponent<RandomDestinationWalk>();
	}

	void Update()
	{
		var posXZ = transform.position.ToXZ();
		var playerPosXZ = this.player.transform.position.ToXZ();
		if (Vector2.Distance(posXZ, playerPosXZ) <= this.minProximity)
		{
			StartFacingPlayer();
			UpdateRotation(posXZ, playerPosXZ);
		}
		else if (this.isFacingPlayer)
		{
			StopFacingPlayer();
		}
	}

	private void StartFacingPlayer()
	{
		this.walkBehaviour.isStopped = true;
		this.isFacingPlayer = true;
	}

	private void StopFacingPlayer()
	{
		this.walkBehaviour.isStopped = false;
		this.isFacingPlayer = false;
	}

	private void UpdateRotation(Vector2 posXZ, Vector2 playerPosXZ)
	{
		var playerOffset = playerPosXZ - posXZ;

		float angle = Vector2.SignedAngle(playerOffset, transform.forward.ToXZ());
		float appliedAngle = Mathf.Sign(angle) * this.rotationDegreesPerSec * Time.deltaTime;
		appliedAngle = angle < 0 ? Mathf.Max(angle, appliedAngle) : Mathf.Min(angle, appliedAngle);

		transform.Rotate(0, appliedAngle, 0, Space.Self);
	}
}
