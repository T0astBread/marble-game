using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using System.Text.RegularExpressions;

public class TextScreen : MonoBehaviour
{
	public UnityEvent revealStart, revealFinish;
	public UnityEvent revealAccelerationStart, revealAccelerationEnd;
	public UnityEvent<string> emotionChange = new StringUnityEvent();

	private TextMeshProUGUI textMeshPro;

	private bool _accelerateRevealing;
	private bool accelerateRevealing
	{
		get { return _accelerateRevealing; }
		set
		{
			if (value != _accelerateRevealing)
			{
				if (value)
					this.revealAccelerationStart.Invoke();
				else
					this.revealAccelerationEnd.Invoke();
			}
			_accelerateRevealing = value;
		}
	}

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

		string prevEmotion = "neutral";
		this.emotionChange.Invoke(prevEmotion);

		this.textMeshPro.maxVisibleCharacters = 0;
		while (this.textMeshPro.maxVisibleCharacters < this.textMeshPro.textInfo.characterCount)
		{
			this.textMeshPro.maxVisibleCharacters += this.accelerateRevealing ? 5 : 1;
			// this.textMeshPro.maxVisibleCharacters = Mathf.Min(this.textMeshPro.maxVisibleCharacters, this.textMeshPro.textInfo.characterCount);

			if (this.textMeshPro.maxVisibleCharacters < this.textMeshPro.textInfo.characterCount - 1)
			{
				var currEmotion = GetEmotion(this.textMeshPro.maxVisibleCharacters);
				if (prevEmotion != currEmotion)
				{
					this.emotionChange.Invoke(currEmotion);
					prevEmotion = currEmotion;
				}
			}

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

	private IEnumerable<TMP_LinkInfo> GetHighestMatchingLinks(int charIndex)
	{
		return this.textMeshPro.textInfo.linkInfo
			.Where(l =>
				l.linkTextfirstCharacterIndex <= charIndex &&
				l.linkTextfirstCharacterIndex + l.linkTextLength >= charIndex
			)
			.OrderByDescending(l => l.linkIdFirstCharacterIndex);
	}

	private string GetEmotion(int charIndex)
	{
		var emotion = GetHighestMatchingLinks(charIndex)
			.Select(l => Regex.Match(l.GetLinkID(), "^emotion_(\\w+)$"))
			.Where(m => m != null)
			.Select(m => m.Groups[1].Value)
			.FirstOrDefault();
		return emotion ?? "neutral";
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

public class StringUnityEvent : UnityEvent<string> { }
