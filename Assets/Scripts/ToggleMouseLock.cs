﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleMouseLock : MonoBehaviour
{
	public string mouseLockToggleKey = "l";

	void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
	}

	void Update()
	{
		Cursor.visible = Cursor.lockState != CursorLockMode.Locked;
		if (Input.GetKeyDown(this.mouseLockToggleKey))
		{
			Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
		}
	}
}
