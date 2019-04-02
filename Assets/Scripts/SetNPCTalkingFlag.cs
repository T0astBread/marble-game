using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetNPCTalkingFlag : MonoBehaviour
{
	private Animator animator;
	private DialogSpawner dialogSpawner;

	void Start()
	{
		this.animator = GetComponent<Animator>();

		this.dialogSpawner = GetComponent<DialogSpawner>();
		if (this.dialogSpawner != null)
		{
			var dialog = this.dialogSpawner.dialog;
			if (dialog != null)
			{
				dialog.dialogStart.AddListener(OnDialogStart);
				dialog.dialogFinish.AddListener(ONDialogFinish);

				var textScreens = dialog.screenContainer.GetComponentsInChildren<TextScreen>(true);
				if (textScreens != null)
				{
					foreach (var textScreen in textScreens)
					{
						textScreen.revealStart.AddListener(OnTalkingStart);
						textScreen.revealFinish.AddListener(OnTalkingEnd);
						textScreen.emotionChange.AddListener(OnEmotionChange);
					}
				}
			}
		}
	}

	private void OnDialogStart()
	{
		this.animator.SetBool("dialog_is_active", true);
		this.animator.SetInteger("talking_emotion", 0);
	}

	private void ONDialogFinish()
	{
		this.animator.SetBool("dialog_is_active", false);
	}

	public void OnTalkingStart()
	{
		this.animator.SetBool("is_talking", true);
	}

	public void OnTalkingEnd()
	{
		this.animator.SetBool("is_talking", false);
	}

	public void OnEmotionChange(string newEmotion)
	{
		int emotionID = 0;
		switch (newEmotion)
		{
			case "angry":
				emotionID = 1;
				break;
			case "shocked":
				emotionID = 2;
				break;
		}
		this.animator.SetInteger("talking_emotion", emotionID);
	}
}
