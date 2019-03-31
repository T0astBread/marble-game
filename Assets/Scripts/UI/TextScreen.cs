using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class TextScreen : MonoBehaviour
{
	public UnityEvent revealStart, revealFinish;

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
		this.revealStart.Invoke();

		this.textMeshPro.maxVisibleCharacters = 0;
		while (this.textMeshPro.maxVisibleCharacters < this.textMeshPro.textInfo.characterCount)
		{
			this.textMeshPro.maxVisibleCharacters += this.accelerateRevealing ? 5 : 1;
			// this.textMeshPro.maxVisibleCharacters = Mathf.Min(this.textMeshPro.maxVisibleCharacters, this.textMeshPro.textInfo.characterCount);

			if (this.accelerateRevealing)
			{
				yield return new WaitForSecondsRealtime(.005f);
			}
			else
			{
				yield return new WaitForSecondsRealtime(.05f);
			}
		}

		this.revealFinish.Invoke();
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
