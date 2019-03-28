using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UpdateNavMeshAgentVelocity : MonoBehaviour
{
	private new Rigidbody rigidbody;
	private NavMeshAgent agent;

	void Start()
	{
		this.rigidbody = GetComponent<Rigidbody>();
		this.agent = GetComponent<NavMeshAgent>();
	}

	void Update()
	{
		this.agent.velocity = this.rigidbody.velocity;
	}
}
