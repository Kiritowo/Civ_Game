using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityNamePlate : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject.FindObjectOfType<HexMap> ().OnCityCreated += CreateCityNamePlate;
	}

	void OnDestroy()
	{
		GameObject.FindObjectOfType<HexMap> ().OnCityCreated -= CreateCityNamePlate;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public GameObject CityNamePlatePrefab;

	public void CreateCityNamePlate(City city, GameObject cityGo)
	{
		GameObject nameGO = (GameObject)Instantiate (CityNamePlatePrefab, this.transform);
		nameGO.GetComponent<MapObjectNamePlate> ().Target = cityGo;
		nameGO.GetComponentInChildren<CityNamePlate> ().MyCity= city;
	}
}
