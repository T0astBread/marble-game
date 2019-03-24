using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DialogSpawner : MonoBehaviour
{
	void StartInteraction(GameObject interactor)
	{
		Debug.Log("*Dialog appears, triggered by" + interactor.name + "*");
	}
}
