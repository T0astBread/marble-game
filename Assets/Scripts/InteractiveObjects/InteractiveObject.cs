using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractiveObject : MonoBehaviour
{
	private Animator animator;

	protected GameObject lastInteractor;

	public virtual void Start()
	{
		this.animator = GetComponent<Animator>();
	}

	public void StartInteraction(GameObject interactor)
	{
		this.lastInteractor = interactor;
		interactor.BroadcastMessage("OnInteractionStart", SendMessageOptions.DontRequireReceiver);
		Interact(interactor);
	}

	protected abstract void Interact(GameObject interactor);

	protected void ReportInteractionFinish()
	{
		if (this.lastInteractor != null)
		{
			this.lastInteractor.BroadcastMessage("OnInteractionFinish", SendMessageOptions.DontRequireReceiver);
		}
	}

	public void ExitInteractiveZone()
	{
		this.animator.SetBool("player_is_in_interactive_zone", false);
	}
}
