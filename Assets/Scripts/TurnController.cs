using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		hexMap = GameObject.FindObjectOfType<HexMap> ();
	}

	HexMap hexMap;

	void EndTurnButton()
	{
		Unit[] units = hexMap.CurrentPlayer.Units;
		City[] cities = hexMap.CurrentPlayer.Cities;
		foreach (Unit i in units) 
		{
			i.RefillAllowance ();
		}

		foreach (City c in cities) 
		{
			c.EndTurn ();
		}
	}
}
