using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroadcastOnCollision : MonoBehaviour
{
	public string enterMessage, exitMessage;

	void OnTriggerEnter(Collider collider)
	{
		collider.BroadcastMessage(this.enterMessage, gameObject, SendMessageOptions.DontRequireReceiver);
	}
	
	void OnTriggerExit(Collider collider)
	{
		collider.BroadcastMessage(this.exitMessage, gameObject, SendMessageOptions.DontRequireReceiver);
	}
}
