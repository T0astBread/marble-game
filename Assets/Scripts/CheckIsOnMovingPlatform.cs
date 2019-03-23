using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckIsOnMovingPlatform : MonoBehaviour
{
	public MovingPlatform platform;
	public float lastExitTime;


	public bool isOnMovingPlatform
	{
		get
		{
			return this.platform != null;
		}
	}


	void OnMovingPlatformEnter(GameObject platformObj)
	{
		this.platform = platformObj.GetComponentInParent<MovingPlatform>();
	}

	void OnMovingPlatformExit(GameObject platformObj)
	{
		this.lastExitTime = Time.time;
		this.platform = null;
	}
}
