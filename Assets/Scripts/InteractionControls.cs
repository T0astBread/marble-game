using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionControls : MonoBehaviour
{
	private InteractiveObject currentInteractiveObject;
	private JumpControls jumpControls;

	void Start()
	{
		this.jumpControls = GetComponent<JumpControls>();
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space) && this.currentInteractiveObject != null)
		{
			Debug.Log(this.currentInteractiveObject.name);
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

		if (this.jumpControls != null)
		{
			this.jumpControls.enabled = true;
		}
	}

	void OnInteractionFinish()
	{
		this.currentInteractiveObject.ExitInteractiveZone();
		this.currentInteractiveObject = null;
	}
}
