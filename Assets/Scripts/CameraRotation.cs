using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
	private Transform playerTransform;
	private MovementControls playerControls;

	void Start()
	{
		var player = GameObject.Find("Marble");
		this.playerTransform = player.transform;
		this.playerControls = player.GetComponent<MovementControls>();
	}

	void Update()
	{
		UpdateCameraRotation();
		UpdatePlayerForward();
	}

	private void UpdateCameraRotation()
	{
		var angleX = transform.rotation.eulerAngles.x - Input.GetAxis("Mouse Y");
		var angleY = transform.rotation.eulerAngles.y + Input.GetAxis("Mouse X");
		transform.rotation = Quaternion.Euler(angleX, angleY, 0);
	}

	private void UpdatePlayerForward()
	{
		this.playerControls.forwardAngle = -Vector3.SignedAngle(Vector3.forward, transform.forward, Vector3.up) - 180;
	}
}
