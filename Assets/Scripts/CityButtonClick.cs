using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityButtonClick {

	public void BuildCity()
	{
		City city = new City();
		HexMap map = GameObject.FindObjectOfType<HexMap> ();
		MouseController m = GameObject.FindObjectOfType<MouseController> ();
		map.SpawnCityAt (city, map.CityPrefab, m.Selected.Hex.A, m.Selected.Hex.B);
	}

}
