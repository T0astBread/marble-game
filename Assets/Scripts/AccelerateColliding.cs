using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccelerateColliding : MonoBehaviour {
	public float factor = 10;

	void OnCollisionStay(Collision collision) {
		var rigidbody = collision.gameObject.GetComponent<Rigidbody>();

		if (rigidbody != null) {
			Debug.Log(rigidbody.velocity * Time.deltaTime * this.factor);
			rigidbody.AddForce(rigidbody.velocity * Time.deltaTime * this.factor, ForceMode.Acceleration);
		}
	}
}
