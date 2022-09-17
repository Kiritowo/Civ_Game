using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CityNamePlateButton : MonoBehaviour, IPointerClickHandler {

	public City MyCity;

	public void OnPointerClick(PointerEventData eventData)
	{
		//MapObjectNamePlate monp = GetComponent<MapObjectNamePlate> ();
		GameObject.FindObjectOfType<CameraController> ().SelectedCity = MyCity;
	}
}
