using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionControls : MonoBehaviour
{
	private GameObject currentInteractableObject;
	private JumpControls jumpControls;

	void Start()
	{
		this.jumpControls = GetComponent<JumpControls>();
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space) && this.currentInteractableObject != null)
		{
			Debug.Log(this.currentInteractableObject.name);
			this.currentInteractableObject.SendMessageUpwards("StartInteraction", gameObject);
		}
	}

	void OnInteractiveObjectEnter(GameObject interactiveObject)
	{
		this.currentInteractableObject = interactiveObject;

		if (this.jumpControls != null)
		{
			this.jumpControls.enabled = false;
		}
	}

	void OnInteractiveObjectExit(GameObject interactiveObject)
	{
		this.currentInteractableObject = null;

		if (this.jumpControls != null)
		{
			this.jumpControls.enabled = true;
		}
	}
}
