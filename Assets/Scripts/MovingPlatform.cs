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

	[HideInInspector]
	public Vector3 impulseToBeApplied;
	[HideInInspector]
	public float currentProgress;
	[HideInInspector]
	public bool isMovingForward;

	private Rigidbody rigidbody;
	private CollisionCache forwardPushColliders, backwardPushColliders;
	private Dictionary<GameObject, float> impulseAmplifications = new Dictionary<GameObject, float>();

	IEnumerator Start()
	{
		this.rigidbody = GetComponent<Rigidbody>();
		this.forwardPushColliders = FindPushColliders("Forward");
		this.backwardPushColliders = FindPushColliders("Backward");

		var start = transform.position;

		while (true)
		{
			this.isMovingForward = true;
			yield return DoDirectionalStep(this.startSleepTime, this.forwardPushColliders, this.destinationOffset, this.forwardDuration, this.forwardImpulseStrength);
			transform.position = start + this.destinationOffset;

			this.isMovingForward = false;
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
		this.impulseToBeApplied = (offset / movementDuration) * impulseStrength;
		yield return Move(pushColliders, offset, movementDuration);

		if (pushColliders != null)
		{
			AddImpulseToObjectsOnPlatform(pushColliders, this.impulseToBeApplied);
		}
	}

	private IEnumerator Move(CollisionCache pushColliders, Vector3 offset, float movementDuration)
	{
		float startTime = Time.time;
		float currentTime = 0;
		while (true)
		{
			currentTime = Time.time - startTime;
			this.currentProgress = currentTime / movementDuration;

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
		this.currentProgress = 0;
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
		{
			float amplification;
			if (!this.impulseAmplifications.TryGetValue(collider.gameObject, out amplification))
			{
				amplification = 1;
			}
			Debug.Log("Impulse amp for " + collider.name + ": " + amplification);

			impulse *= amplification;
			colliderBody.AddForce(impulse, ForceMode.Impulse);

			collider.BroadcastMessage("OnMovingPlatformImpulse", new object[] { impulse, amplification }, SendMessageOptions.DontRequireReceiver);
		});

		this.impulseAmplifications.Clear();
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

	public void RequestImpulseAmplification(GameObject target, float amplification)
	{
		this.impulseAmplifications[target] = amplification;
	}
}
