using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetNPCTalkingFlag : MonoBehaviour
{
	private Animator animator;

	void Start()
	{
		this.animator = GetComponent<Animator>();
	}

	public void OnTalkingStart()
	{
		this.animator.SetBool("is_talking", true);
	}

	public void OnTalkingEnd()
	{
		this.animator.SetBool("is_talking", false);
	}
}
