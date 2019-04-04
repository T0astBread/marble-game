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
				dialog.dialogFinish.AddListener(OnDialogFinish);

				var textScreens = dialog.screenContainer.GetComponentsInChildren<TextScreen>(true);
				if (textScreens != null)
				{
					foreach (var textScreen in textScreens)
					{
						textScreen.revealStart.AddListener(OnTalkingStart);
						textScreen.revealFinish.AddListener(OnTalkingEnd);
						textScreen.revealAccelerationStart.AddListener(OnRevealAccelerationStart);
						textScreen.revealAccelerationEnd.AddListener(OnRevealAccelerationEnd);
						textScreen.emotionChange.AddListener(OnEmotionChange);
					}
				}
			}
		}

		this.animator.SetLayerWeight(this.animator.GetLayerIndex("TalkingEmotionLayer"), 0);
	}

	private void OnDialogStart()
	{
		this.animator.SetBool("dialog_is_active", true);
		this.animator.SetInteger("talking_emotion", 0);
		this.animator.SetLayerWeight(this.animator.GetLayerIndex("TalkingEmotionLayer"), 1);
	}

	private void OnDialogFinish()
	{
		this.animator.SetBool("dialog_is_active", false);
		this.animator.SetLayerWeight(this.animator.GetLayerIndex("TalkingEmotionLayer"), 0);
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

	public void OnRevealAccelerationStart()
	{
		this.animator.SetBool("talking_is_accelerated", true);
	}

	public void OnRevealAccelerationEnd()
	{
		this.animator.SetBool("talking_is_accelerated", false);
	}
}
