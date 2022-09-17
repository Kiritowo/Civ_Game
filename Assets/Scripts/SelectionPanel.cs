using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionPanel : MonoBehaviour {

	// Use this for initialization
	void Start () {
		mouseContols = GameObject.FindObjectOfType<CameraController> ();
	}

	public Text currentUnit;
	public Text Movement;
	public GameObject CityButton;
	CameraController mouseContols;
	
	// Update is called once per frame
	void Update () {
		if (mouseContols.SelectedUnit != null)
		{
			currentUnit.text = mouseContols.SelectedUnit.Name;
			Movement.text = string.Format("{0}/{1}",mouseContols.SelectedUnit.AllowanceLeft, mouseContols.SelectedUnit.Allowance);
			if (mouseContols.SelectedUnit.CityBuilder && mouseContols.SelectedUnit.Hex.City == null) 
			{
				CityButton.SetActive (true);
			}
			else
			{
				CityButton.SetActive (false);
			}
		}
	}
}
