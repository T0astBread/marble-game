using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomDestinationWalk : MonoBehaviour
{
	public float minSleepTime, maxSleepTime;
	public float maxHomeDistance;

	private NavMeshAgent agent;
	private Vector3 home;

	private bool _isStopped;
	public bool isStopped
	{
		get { return _isStopped; }
		set
		{
			_isStopped = value;
			this.agent.isStopped = value;
		}
	}

	IEnumerator Start()
	{
		this.agent = GetComponent<NavMeshAgent>();
		this.home = transform.position;

		while (true)
		{
			yield return new WaitForSeconds(Random.Range(this.minSleepTime, this.maxSleepTime));

			if (this.isStopped)
			{
				continue;
			}

			var destinationOffsetXZ = Random.insideUnitCircle * Random.value * this.maxHomeDistance;
			var destination = this.home + new Vector3(destinationOffsetXZ.x, 0, destinationOffsetXZ.y);
			this.agent.SetDestination(destination);
		}
	}
}
