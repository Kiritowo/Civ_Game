using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MapObjectNamePlate : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (TheCamera == null)
			TheCamera = Camera.main;
		rectTransform = GetComponent<RectTransform> ();
	}

	public GameObject Target;
	public Vector3 WorldPositionOffset = new Vector3(0, 1, 0);
	public Vector3 PositionOffset = new Vector3(0, 30, 0);
	public Camera TheCamera;
	RectTransform rectTransform;
	
	// Update is called once per frame
	void LateUpdate () {
		if (Target == null) 
		{
			Destroy (gameObject);
			return;
		}
		Vector3 screenpos = TheCamera.WorldToScreenPoint (Target.transform.position + WorldPositionOffset);
		rectTransform.anchoredPosition = screenpos + PositionOffset;
	}
}
