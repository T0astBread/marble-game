using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(SlowMotion))]
public class DialogSpawner : InteractiveObject
{
	public DialogBox dialog;
	public bool slowTime, disableMovement;

	private SlowMotion slowMotion;

	override public void Start()
	{
		base.Start();
		this.slowMotion = GetComponent<SlowMotion>();
	}

	override protected void Interact(GameObject interactor)
	{
		Debug.Log("A dialog was triggered by " + interactor.name);
		this.dialog.gameObject.SetActive(true);

		if (this.slowTime)
		{
			StartCoroutine(this.slowMotion.GraduallySetTimeScale(.05f, 1));
		}
		
		if (this.disableMovement)
		{
			interactor.SendMessage("DisableMovement");
		}
	}

	void OnDialogFinish()
	{
		if (this.slowTime)
		{
			StartCoroutine(this.slowMotion.GraduallySetTimeScale(1, 1));
		}

		if (this.disableMovement)
		{
			this.lastInteractor.SendMessage("EnableMovement");
		}

		ReportInteractionFinish();
	}
}
