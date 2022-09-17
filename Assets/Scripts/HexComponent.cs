using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexComponent : MonoBehaviour {

	public Hex Hex;
	public HexMap HexMap;

	public float VerticalOffset = 0;

	public void UpdatePosition()
	{
		this.transform.position = Hex.PosFromCamera (Camera.main.transform.position,
			HexMap.mapWidth,
			HexMap.mapHeight
			);
	}
}
