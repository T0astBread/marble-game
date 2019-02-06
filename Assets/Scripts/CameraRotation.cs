using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
	public float minAngleX, maxAngleX;

	private Transform playerTransform;
	private MovementControls playerControls;
	private float initialAngleX;

	void Start()
	{
		var player = GameObject.Find("Marble");
		this.playerTransform = player.transform;
		this.playerControls = player.GetComponent<MovementControls>();
		this.initialAngleX = transform.rotation.eulerAngles.x;
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

		angleX = Mathf.Clamp(angleX, this.minAngleX, this.maxAngleX);

		transform.rotation = Quaternion.Euler(angleX, angleY, 0);
	}

	private void UpdatePlayerForward()
	{
		this.playerControls.forwardAngle = -Vector3.SignedAngle(Vector3.forward, transform.forward, Vector3.up) - 180;
	}
}
