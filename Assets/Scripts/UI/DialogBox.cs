using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DialogBox : MonoBehaviour
{
	public int currentScreen = -1;

	private Transform screenContainer;
	private bool currentScreenHasFinished;
	private GameObject screenAdvanceIndicator;

	void Start()
	{
		this.screenAdvanceIndicator = transform.GetChild(1).gameObject;
	}

	void OnEnable()
	{
		this.currentScreen = 0;
		this.currentScreenHasFinished = false;
	}

	void Update()
	{
		if (this.screenContainer == null)
		{
			this.screenContainer = transform.GetChild(0);
		}

		UpdateVisibleScreens();
		bool isNotAtLastScreen = this.currentScreen < this.screenContainer.childCount - 1;
		if (this.currentScreenHasFinished && Input.GetKeyDown(KeyCode.Space))
		{
			if (isNotAtLastScreen)
			{
				AdvanceScreen();
			}
			else
			{
				FinishDialog();
			}
		}

		this.screenAdvanceIndicator.SetActive(this.currentScreenHasFinished && isNotAtLastScreen);
	}

	private void UpdateVisibleScreens()
	{
		for (int i = 0; i < this.screenContainer.childCount; i++)
		{
			this.screenContainer.GetChild(i).gameObject.SetActive(this.currentScreen == i);
		}
	}

	private void AdvanceScreen()
	{
		this.currentScreen++;
		UpdateVisibleScreens();
		this.currentScreenHasFinished = false;
	}

	private void FinishDialog()
	{
		gameObject.SetActive(false);
		foreach (var root in gameObject.scene.GetRootGameObjects())
		{
			root.BroadcastMessage("OnDialogFinish");
		}
	}

	void OnScreenFinish()
	{
		this.currentScreenHasFinished = true;
	}
}
