using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DialogSpawner : InteractiveObject
{
	public GameObject dialog;

	override protected void Interact(GameObject interactor)
	{
		Debug.Log("A dialog was triggered by " + interactor.name);
		Time.timeScale = 0;
		this.dialog.SetActive(true);
	}

	void OnDialogFinish()
	{
		Time.timeScale = 1;
		ReportInteractionFinish();
	}
}
