﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TextScreen : MonoBehaviour
{
	private TextMeshProUGUI textMeshPro;
	private bool accelerateRevealing;

	void Awake()
	{
		this.textMeshPro = GetComponent<TextMeshProUGUI>();
	}

	void OnEnable()
	{
		StartCoroutine(Show());
	}

	private IEnumerator Show()
	{
		yield return RevealCharacters();
		SendMessageUpwards("OnScreenFinish");
	}

	private IEnumerator RevealCharacters()
	{
		this.textMeshPro.maxVisibleCharacters = 0;
		while (this.textMeshPro.maxVisibleCharacters < this.textMeshPro.textInfo.characterCount)
		{
			this.textMeshPro.maxVisibleCharacters++;
			yield return new WaitForSecondsRealtime(this.accelerateRevealing ? .005f : .05f);
		}
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			this.accelerateRevealing = true;
		}
		else if (Input.GetKeyUp(KeyCode.Space))
		{
			this.accelerateRevealing = false;
		}
	}
}