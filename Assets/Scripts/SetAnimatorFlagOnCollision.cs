using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAnimatorFlagOnCollision : MonoBehaviour
{
	public string flag;

	private Animator animator;

	void Start()
	{
		this.animator = GetComponentInParent<Animator>();
	}

	void OnTriggerEnter(Collider collider)
	{
		this.animator.SetBool(this.flag, true);
	}
	
	void OnTriggerExit(Collider collider)
	{
		this.animator.SetBool(this.flag, false);
	}
}
