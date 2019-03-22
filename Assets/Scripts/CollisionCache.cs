using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCache : MonoBehaviour
{
	public HashSet<Collider> collidingColliders = new HashSet<Collider>();

	void OnTriggerEnter(Collider collider)
	{
		this.collidingColliders.Add(collider);
	}
	
	void OnTriggerExit(Collider collider)
	{
		this.collidingColliders.Remove(collider);
	}
}
