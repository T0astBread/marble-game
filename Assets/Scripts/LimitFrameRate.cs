using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitFrameRate : MonoBehaviour
{
	public int targetFrameRate = 20;

	void Start()
	{
#if UNITY_EDITOR
		QualitySettings.vSyncCount = 0;
#endif
	}

	void Update()
	{
#if UNITY_EDITOR
		Application.targetFrameRate = this.targetFrameRate;
#endif
	}
}
