using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class StickToPlayer : MonoBehaviour
{
	public Vector3 offset;

	private GameObject player;

	void Start()
	{
		this.player = GameObject.Find("Marble");
	}

	void Update()
	{
		transform.position = this.player.transform.position + this.offset;
	}
}
