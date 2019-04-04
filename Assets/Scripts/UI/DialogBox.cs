using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogBox : MonoBehaviour
{
	public int currentScreen = -1;
	public UnityEvent dialogStart, dialogFinish;

	private bool currentScreenHasFinished;
	private GameObject screenAdvanceIndicator;
	
	private Transform _screenContainer;
	public Transform screenContainer
	{
		get
		{
			InitScreenContainer();
			return _screenContainer;
		}
	}


	void Start()
	{
		this.screenAdvanceIndicator = transform.GetChild(1).gameObject;
	}

	void OnEnable()
	{
		this.currentScreen = 0;
		this.currentScreenHasFinished = false;
		this.dialogStart.Invoke();
	}

	void Update()
	{
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

	private void InitScreenContainer()
	{
		if (this._screenContainer == null)
		{
			this._screenContainer = transform.GetChild(0);
		}
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
			root.BroadcastMessage("OnDialogFinish", SendMessageOptions.DontRequireReceiver);
		}
		this.dialogFinish.Invoke();
	}

	void OnScreenFinish()
	{
		this.currentScreenHasFinished = true;
	}
}
