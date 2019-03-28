using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionControls : MonoBehaviour
{
	private InteractiveObject currentInteractiveObject;
	private JumpControls jumpControls;

	private bool interactionIsBlocked;

	void Start()
	{
		this.jumpControls = GetComponent<JumpControls>();
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space) && this.currentInteractiveObject != null && !this.interactionIsBlocked)
		{
			this.currentInteractiveObject.StartInteraction(gameObject);
		}
	}

	void OnInteractiveObjectEnter(GameObject interactionColliders)
	{
		this.currentInteractiveObject = interactionColliders.GetComponentInParent<InteractiveObject>();

		if (this.jumpControls != null)
		{
			this.jumpControls.enabled = false;
		}
	}

	void OnInteractiveObjectExit(GameObject interactionColliders)
	{
		this.currentInteractiveObject = null;
		EnableJumpControls();
		this.interactionIsBlocked = false;
	}

	void OnInteractionFinish()
	{
		this.currentInteractiveObject.ExitInteractiveZone();
		EnableJumpControls();
		this.interactionIsBlocked = true;
	}

	private void EnableJumpControls()
	{
		if (this.jumpControls != null)
		{
			this.jumpControls.enabled = true;
		}
	}
}
