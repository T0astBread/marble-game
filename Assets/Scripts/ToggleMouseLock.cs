using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleMouseLock : MonoBehaviour
{
	public string mouseLockToggleKey = "l";
	
	void Update()
	{
		if (Input.GetKeyDown(this.mouseLockToggleKey))
		{
			Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
		}
	}
}
