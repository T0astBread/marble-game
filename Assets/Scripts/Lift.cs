using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift : MonoBehaviour
{
	public Vector3 destinationOffset;
	public float forwardDuration, backwardDuration;
	public float startSleepTime, retractSleepTime;
	public float forwardImpulseStrength = 200;

	private Rigidbody rigidbody;

	IEnumerator Start()
	{
		this.rigidbody = GetComponent<Rigidbody>();
		var start = transform.position;

		while (true)
		{
			yield return SleepAndMove(this.startSleepTime, this.destinationOffset, this.forwardDuration);
			transform.position = start + this.destinationOffset;

			var movementSpeedVec = this.destinationOffset / this.forwardDuration;
			AddImpulseToObjectsOnPlatform(movementSpeedVec * this.forwardImpulseStrength);

			yield return SleepAndMove(this.retractSleepTime, -this.destinationOffset, this.backwardDuration);
			transform.position = start;
		}
	}

	private IEnumerator SleepAndMove(float sleepDuration, Vector3 offset, float movementDuration)
	{
		yield return new WaitForSeconds(sleepDuration);
		yield return Move(offset, movementDuration);
	}

	private IEnumerator Move(Vector3 offset, float movementDuration)
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

			MoveObjectsOnPlatform(movement);
			transform.position += movement;

			yield return null;
		}
	}

	private void MoveObjectsOnPlatform(Vector3 movement)
	{
		ForObjectsOnPlatform((collider, colliderBody) =>
		{
			colliderBody.transform.position += movement;
			colliderBody.AddForce(movement, ForceMode.Acceleration);
		});
	}

	private void AddImpulseToObjectsOnPlatform(Vector3 impulse)
	{
		ForObjectsOnPlatform((collider, colliderBody) =>
			colliderBody.AddForce(impulse, ForceMode.Impulse)
		);
	}

	private void ForObjectsOnPlatform(Action<Collider, Rigidbody> action)
	{
		float overlapMargin = .5f;
		var halfOverlapScale = new Vector3(transform.localScale.x / 2 - overlapMargin, .5f, transform.localScale.z / 2 - overlapMargin);
		var overlapCenterPosition = transform.position + transform.up;

		Debug.DrawLine(overlapCenterPosition - halfOverlapScale, overlapCenterPosition + halfOverlapScale, Color.red);

		var collidersOnPlatform = Physics.OverlapBox(overlapCenterPosition, halfOverlapScale, transform.rotation);


		foreach (var collider in collidersOnPlatform)
		{
			if (collider.gameObject == gameObject)
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
