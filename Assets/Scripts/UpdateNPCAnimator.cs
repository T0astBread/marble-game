using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UpdateNPCAnimator : MonoBehaviour
{
	private Animator animator;
	private NavMeshAgent agent;

	void Start()
	{
		this.animator = GetComponent<Animator>();
		this.agent = GetComponent<NavMeshAgent>();
	}

	void Update()
	{
		bool isWalking = this.agent.hasPath && !this.agent.isStopped && this.agent.remainingDistance > this.agent.stoppingDistance;
		this.animator.SetBool("is_walking", isWalking);
	}
}
