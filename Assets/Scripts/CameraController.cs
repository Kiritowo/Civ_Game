using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		current = UpdateStart;
		hexMap = GameObject.FindObjectOfType<HexMap> ();
		lineRenderer = transform.GetComponentInChildren<LineRenderer> ();
	}

	public GameObject UnitSelectionPanel;
	public GameObject CitySelectionPanel;

	HexMap hexMap;
	Hex hexUnder;
	Hex lastHex;
	int mouseThreshold = 1;
	bool isDraggingCamera = false;
	Vector3 lastMousePosition;

	Unit selected = null;
	public Unit SelectedUnit
	{
		get{ return selected; }
		set
		{ 
			selected = null;
			if(SelectedCity != null)
			SelectedCity = null;
			selected = value;
			Selection.SetActive (selected != null);
		}
	}

	City selectedCity = null;
	public City SelectedCity
	{
		get{ return SelectedCity; }
		set
		{ 
			selectedCity = null;
			if(SelectedUnit !=null)
			SelectedUnit = null;
			selectedCity = value;
			Cancel ();
			CitySelectionPanel.SetActive (selected != null);
			current = Update_CityView;
		}
	}

	Hex[] hexPath;
	LineRenderer lineRenderer;

	delegate void Function();
	Function current;

	public LayerMask LayerID;

	void Update()
	{
		hexUnder = MouseHex ();
		if (Input.GetKeyDown (KeyCode.Escape)) 
		{
			Selection = null;
			Cancel ();
		}
		current ();
		lastMousePosition = Input.mousePosition;
		lastHex = hexUnder; 
		if (selected != null) {
			DrawPath ((hexPath != null) ? hexPath : selected.GetHexPath ());
		} 
		else 
		{
			DrawPath (null);
		}
	}

	void Cancel()
	{
		current = UpdateStart;
		selected = null;
		hexPath = null;
	}

	void UpdateStart ()
	{
		if (EventSystem.current.IsPointerOverGameObject ()) 
		{
			return;
		}
		if (Input.GetMouseButtonDown (0)) {
			
		} else if (Input.GetMouseButtonUp (0)) {
			MouseHex ();
			Unit[] you = hexUnder.Units;
			if (you.Length > 0) {
				selected = you [0];
				current = Update_UnitMov;
			}
		} else if (selected != null && Input.GetMouseButtonDown (1)) 
		{
			current = Update_UnitMov;
		}
		else if (Input.GetMouseButton (0) && Vector3.Distance (Input.mousePosition, lastMousePosition) > mouseThreshold) {
			current = UpdateCamera;
			lastMousePosition = lastMousePosition = MouseToPlane (Input.mousePosition);
			current ();
		} 
		else if (selected != null && Input.GetMouseButton (2)) 
		{
			
		}
	}

	Hex MouseHex()
	{
		Ray mouseRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit HitInfo;
		int layerMask = LayerID.value;
		if (Physics.Raycast (mouseRay, out HitInfo, Mathf.Infinity, LayerID)) 
		{
			GameObject hexGO = HitInfo.rigidbody.gameObject;
			return hexMap.GetHexFromGO (hexGO);
		}

	}

	Vector3 MouseToPlane(Vector3 mousePos)
	{
		Ray mouseRay = Camera.main.ScreenPointToRay (mousePos);
		if (mouseRay.direction.y >= 0) 
		{
			return;
		}
		float rayLength = (mouseRay.origin.y / mouseRay.direction.y);
		return mouseRay.origin - (mouseRay.direction * rayLength);
	}

	void Update_UnitMov()
	{
		if (Input.GetMouseButtonUp (1) || selected == null) 
		{
			if (selected != null) 
			{
				selected.setPath (hexPath);
				StartCoroutine (hexMap.SingleMove (selected));
			}
			Cancel ();
			return;
		}
		if (hexPath == null || hexUnder != lastHex) 
		{
			hexPath = Pathfinding.Pathfinding.FindPath<Hex> (hexMap, selected, selected.Hex, hexUnder, Hex.CostEst);
		}
	}

	void DrawPath(Hex[] hexPath)
	{
		if (hexPath == null || hexPath.Length == 0) 
		{
			lineRenderer.enabled = false;
			return;
		}
		lineRenderer.enabled = true;
		Vector3[] poss = new Vector3[hexPath.Length];
		for(int i = 0; i < hexPath.Length; i++)
		{
			GameObject hexGO = hexMap.GetHexGO (hexPath[i]);
			poss [i] = hexGO.transform.position + (Vector3.up*0.1f);
		}
		lineRenderer.positionCount = poss.Length;
		lineRenderer.SetPositions (poss);
	}

	public GameObject Selection;

	void UpdateCamera () 
	{
		if (Input.GetMouseButtonUp (0)) 
		{
			Cancel ();
			return;
		}
		Ray mouseRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		if (mouseRay.direction.y >= 0) 
		{
			Debug.LogError ("?");
			return;
		}
		float rayLength = (mouseRay.origin.y / mouseRay.direction.y);
		Vector3 hitPos = MouseToPlane(Input.mousePosition);

		lastMousePosition = hitPos;

		if (Input.GetMouseButtonDown (0)) {
			isDraggingCamera = true;

			lastMousePosition = hitPos;
		} 
		else if (Input.GetMouseButtonUp (0))
		{
			isDraggingCamera = false;
		}

		if (isDraggingCamera) 
		{
			Vector3 difference = lastMousePosition - hitPos;
			Camera.main.transform.Translate (difference, Space.World);
			mouseRay = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (mouseRay.direction.y >= 0) 
			{
				Debug.LogError ("?");
				return;
			}
			rayLength = (mouseRay.origin.y / mouseRay.direction.y);
			lastMousePosition = mouseRay.origin - (mouseRay.direction * rayLength);
		}
		//Controls zoom via scrolling
		float Scroll = -Input.GetAxis ("ScrollWheel");
		if (Mathf.Abs (Scroll) > 0.01f) 
		{
			Vector3 distance = Camera.main.transform.position - hitPos;
			Camera.main.transform.Translate(distance * Scroll, Space.World);
			Vector3 cameraPos = Camera.main.transform.position;
			if (cameraPos.y < 2) 
			{
				cameraPos.y = 2;
			}
			if (cameraPos.y > 20) 
			{
				cameraPos.y = 20;
			}
			Camera.main.transform.position = cameraPos;
		}
	}

	void Update_CityView()
	{
		UpdateStart ();
	}
}
