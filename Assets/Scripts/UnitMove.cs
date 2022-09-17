using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMove : MonoBehaviour {

	void Start()
	{
		newPosition = this.transform.position;
	}
	//Sets variables for movement
	Vector3 oldPosition;
	Vector3 newPosition;
	Vector3 Velocity;
	float smoothing = 0.5f;

	//Records the current hex tile position relative to old hex tile position
	public void OnUnitMoved(Hex oldHex, Hex newHex)
	{
		Vector3 oldPosition = oldHex.PosFromCamera ();
		newPosition = newHex.PosFromCamera ();
		Velocity = Vector3.zero;

		oldPosition.y += newHex.HexMap.GetHexGO (oldHex).GetComponent<HexComponent> ().VerticalOffset;
		newPosition.y += newHex.HexMap.GetHexGO (newHex).GetComponent<HexComponent> ().VerticalOffset;
		this.transform.position = oldPosition;

		if (Vector3.Distance (this.transform.position, newPosition) > 1) {
			this.transform.position = newPosition;
		} 
		else 
		{
			GameObject.FindObjectOfType<HexMap> ().animationPlaying = true;
		}
	}
	//Moves the object over a period of 0.5 seconds
	void Update()
	{
		this.transform.position = Vector3.SmoothDamp(this.transform.position, newPosition, ref Velocity, smoothing);
		if (Vector3.Distance (this.transform.position, newPosition) < 0.1f) 
		{
			GameObject.FindObjectOfType<HexMap> ().animationPlaying = false;
		}
	}
}
