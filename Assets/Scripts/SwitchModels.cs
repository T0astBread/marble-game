using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SwitchModels : MonoBehaviour {
	public GameObject modelContainer;
	public GameObject[] models;
	public int currentModelIndex = -1;

	private int lastFrameModelIndex = -1;
	private GameObject modelObject;

	void Update()
	{
		SelectModelContainerIfNull();
		DestroyExcessChildren();

		if (this.currentModelIndex == -1)
		{
			return;
		}

		UpdateModel();
	}

	private void SelectModelContainerIfNull()
	{
		if (this.modelContainer == null)
		{
			this.modelContainer = transform.GetChild(0).gameObject;
		}
	}

	private void DestroyExcessChildren()
	{
		int childCount = this.modelContainer.transform.childCount;
		if (childCount > 1)
		{
			for (int i = 0; i < childCount; i++)
			{
				var child = this.modelContainer.transform.GetChild(i);
				if(child != this.modelObject)
				{
					GameObject.DestroyImmediate(child.gameObject);
					childCount--;
				}
			}
		}
	}

	private void UpdateModel()
	{
		if (this.currentModelIndex != this.lastFrameModelIndex)
		{
			Debug.Log("Switching marble model from " + this.lastFrameModelIndex + " to " + this.currentModelIndex);
			DestroyModel();
			InstantiateModel();
		}
		this.lastFrameModelIndex = this.currentModelIndex;
	}

	private void DestroyModel()
	{
		if (this.modelObject != null)
		{
			GameObject.DestroyImmediate(this.modelObject);
			this.modelObject = null;
		}
	}

	private void InstantiateModel()
	{
		if(this.currentModelIndex >= this.models.Length)
		{
			return;
		}
		
		var modelPrefab = this.models[this.currentModelIndex];
		if (modelPrefab != null)
		{
			this.modelObject = GameObject.Instantiate(modelPrefab, transform.position, transform.rotation);
			this.modelObject.transform.parent = this.modelContainer.transform;
		}
	}
}
