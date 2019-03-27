using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMotion : MonoBehaviour
{
	public void SetTimeScale(float scale)
	{
		float timeScaleChange = scale / Time.timeScale;
		Time.timeScale = scale;
		Time.fixedDeltaTime *= timeScaleChange;

		Debug.Log("Setting slow motion; timeScale: " + scale + "; fixedDeltaTime: " + Time.fixedDeltaTime);
	}

	public IEnumerator GraduallySetTimeScale(float targetScale, float duration)
	{
		float startTimeScale = Time.timeScale;
		float changeSpan = targetScale - Time.timeScale;
		float startTime = Time.unscaledTime;
		while (true)
		{
			float runTime = Time.unscaledTime - startTime;
			if(runTime >= duration)
			{
				break;
			}

			float progress = runTime / duration;
			SetTimeScale(startTimeScale + changeSpan * progress);

			yield return null;
		}
		SetTimeScale(targetScale);
	}
}
