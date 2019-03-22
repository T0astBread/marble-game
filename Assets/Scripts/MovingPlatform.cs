using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
	public Vector3 destinationOffset;
	public float forwardDuration, backwardDuration;
	public float startSleepTime, retractSleepTime;
	public float forwardImpulseStrength = 200;
	public float backwardImpulseStrength = 200;

	private Rigidbody rigidbody;
	private CollisionCache forwardPushColliders, backwardPushColliders;

	IEnumerator Start()
	{
		this.rigidbody = GetComponent<Rigidbody>();
		this.forwardPushColliders = FindPushColliders("Forward");
		this.backwardPushColliders = FindPushColliders("Backward");

		var start = transform.position;

		while (true)
		{
			yield return DoDirectionalStep(this.startSleepTime, this.forwardPushColliders, this.destinationOffset, this.forwardDuration, this.forwardImpulseStrength);
			transform.position = start + this.destinationOffset;

			yield return DoDirectionalStep(this.retractSleepTime, this.backwardPushColliders, -this.destinationOffset, this.backwardDuration, this.backwardImpulseStrength);
			transform.position = start;
		}
	}

	private CollisionCache FindPushColliders(string name)
	{
		return GetComponentsInChildren<CollisionCache>()
			.Where(c => c.gameObject.name == "PushColliders_" + name)
			.FirstOrDefault();
	}

	private IEnumerator DoDirectionalStep(float sleepDuration, CollisionCache pushColliders, Vector3 offset, float movementDuration, float impulseStrength)
	{
		yield return new WaitForSeconds(sleepDuration);
		yield return Move(pushColliders, offset, movementDuration);

		if (pushColliders != null)
		{
			var movementSpeedVec = offset / movementDuration;
			AddImpulseToObjectsOnPlatform(pushColliders, movementSpeedVec * impulseStrength);
		}
	}

	private IEnumerator Move(CollisionCache pushColliders, Vector3 offset, float movementDuration)
	{
		float startTime = Time.time;
		float currentTime = 0;
		while (true)
		{
			currentTime = Time.time - startTime;
			if (currentTime >= movementDuration)
			{
				break;
			}

			float deltaProgress = Time.deltaTime / movementDuration;
			var movement = offset * deltaProgress;

			if (pushColliders != null)
			{
				MoveObjectsOnPlatform(pushColliders, movement);
			}
			transform.position += movement;

			yield return null;
		}
	}

	private void MoveObjectsOnPlatform(CollisionCache pushColliders, Vector3 movement)
	{
		ForObjectsOnPlatform(pushColliders, (collider, colliderBody) =>
		{
			colliderBody.transform.position += movement;
			colliderBody.AddForce(movement, ForceMode.Acceleration);
		});

		Debug.DrawLine(transform.position, transform.position + movement, Color.yellow, .1f);
		Debug.DrawLine(transform.position + movement, transform.position + movement + Vector3.up, Color.blue, .1f);
	}

	private void AddImpulseToObjectsOnPlatform(CollisionCache pushColliders, Vector3 impulse)
	{
		ForObjectsOnPlatform(pushColliders, (collider, colliderBody) =>
			colliderBody.AddForce(impulse, ForceMode.Impulse)
		);
	}

	private void ForObjectsOnPlatform(CollisionCache colliders, Action<Collider, Rigidbody> action)
	{
		var collidersOnPlatform = colliders.collidingColliders;

		foreach (var collider in collidersOnPlatform)
		{
			if (collider.gameObject == gameObject || collider.GetComponent<MovingPlatform>() != null)
			{
				continue;
			}

			var colliderBody = collider.GetComponent<Rigidbody>();
			if (colliderBody != null)
			{
				action(collider, colliderBody);
			}
		}
	}
}
