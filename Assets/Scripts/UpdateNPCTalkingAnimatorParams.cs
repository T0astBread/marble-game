using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateNPCTalkingAnimatorParams : MonoBehaviour
{
	private const int EMOTION_NEUTRAL = 0;

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

		SetTalkingEmotionLayerActive(false);
	}

	private void OnDialogStart()
	{
		this.animator.SetBool("dialog_is_active", true);
		SetTalkingEmotion(EMOTION_NEUTRAL);
		SetTalkingEmotionLayerActive(true);
	}

	private void OnDialogFinish()
	{
		this.animator.SetBool("dialog_is_active", false);
		SetTalkingEmotion(EMOTION_NEUTRAL);
		SetTalkingEmotionLayerActive(false);
	}

	private void SetTalkingEmotionLayerActive(bool active)
	{
		this.animator.SetLayerWeight(this.animator.GetLayerIndex("TalkingEmotionLayer"), active ? 1 : 0);
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
		SetTalkingEmotion(newEmotion);
	}

	private void SetTalkingEmotion(string emotionName)
	{
		int emotionID = 0;
		switch (emotionName)
		{
			case "angry":
				emotionID = 1;
				break;
			case "shocked":
				emotionID = 2;
				break;
		}
		SetTalkingEmotion(emotionID);
	}

	private void SetTalkingEmotion(int emotionID)
	{
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
