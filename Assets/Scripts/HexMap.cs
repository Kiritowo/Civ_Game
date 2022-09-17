using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathFinding;
using System.Linq;

public class HexMap : MonoBehaviour, IPathWorld {

	// This starts the funtion to generate a map
	void Start () {

		GeneratePlayers (2);
		GenerateMap ();
	}

	public bool animationPlaying = false;
	public delegate void CityCreatedDelegate(City city, GameObject cityGo);
	public event CityCreatedDelegate OnCityCreated;
	void GeneratePlayers(int numPlayers)
	{
		Players = new Player[numPlayers];
		for (int i = 0; i < numPlayers; i++) 
		{
			Players [i] = new Player ("Player " + (i+1).ToString ());
		}
		CurrentPlayer = Players [0];
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space)) 
		{
			StartCoroutine (AllMoves);
		}
	}

	IEnumerator AllMoves()
	{
			foreach (Unit i in CurrentPlayer.Units) 
			{
				yield return SingleMove (i); 
			}
	}

	public IEnumerator SingleMove(Unit i)
	{
		while (i.Turn ()) 
		{
			while (animationPlaying) 
			{
				yield return null;
			}
		}
	}

	//This refers to the gameobject being used to generate the map
	public GameObject HexPrefab;

	public Mesh MeshWater;
	public Mesh MeshFlat;
	public Mesh MeshHill;
	public Mesh MeshMountain;

	public Material MatOcean;
	public Material MatPlains;
	public Material MatGrass;
	public Material MatMountains;
	public Material MatDesert;

	public GameObject UnitKnightPrefab;
	public GameObject CityPrefab;

	[System.NonSerialized] public float HeightMountain = 1f;
	[System.NonSerialized] public float HeightHill = 0.5f;
	[System.NonSerialized] public float HeightFlat = 0f;

	[System.NonSerialized] public int mapHeight = 20;
	[System.NonSerialized] public int mapWidth = 40;

	[System.NonSerialized] public float MoistureJungle = 1f;
	[System.NonSerialized] public float MoistureForest = 0.5f;
	[System.NonSerialized] public float MoistureGrass = 0.25f;
	[System.NonSerialized] public float MoisturePlains = 0f;

	private Hex[,] hexes;
	private Dictionary<Hex, GameObject> hextoGameObjectMap;
	private Dictionary<GameObject, Hex> gameObjectToHexMap;

	public Player[] Players;
	private Player player;
	public Player CurrentPlayer
	{
		get{ return player; }
		set{ player = value; }
	}

	private Dictionary<Unit, GameObject> unitToGameObjectMap;
	private Dictionary<Unit, GameObject> cityToGameObjectMap;


	public Hex GetHexAt(int x, int y)
	{
		if (hexes == null)
		{
			Debug.LogError ("Break");
			return null;
		}

		if (allowWrapEastWest) 
		{
			x = x % HexHeight;
			if (x < 0) 
			{
				x += HexHeight;
			}
		}

		if (allowWrapNorthSouth) 
		{
			y = y % HexWidth;
			if (y < 0) 
			{
				y += HexWidth;
			}
		}
		try
		{
			return hexes [x, y];
		}
		catch
		{
			Debug.LogError ("GetHexAt: " + x + "," + y);
			return null;
		}
	}
	public Hex GetHexFromGO(GameObject HexGo)
	{
		if (gameObjectToHexMap.ContainsKey (HexGo)) 
		{
			return gameObjectToHexMap [HexGo];
		}
		return null;
	}
	public GameObject GetHexGO(Hex h)
	{
		if (gameObjectToHexMap.ContainsKey (h)) 
		{
			return gameObjectToHexMap [h];
		}
		return null;
	}
	public Hex GetHexPosition(int a, int b)
	{
		Hex h = GetHexAt (a, b);
		return GetHexPosition (h);
	}

	public Vector3 GetHexPosition(Hex hex)
	{
		return hex.PosFromCamera (Camera.main.transform.position, mapWidth, mapHeight);
	}

	//This allows the objects to be generated
	virtual public void GenerateMap()
	{

		hexes = new Hex[mapHeight,mapWidth];
		hextoGameObjectMap = new Dictionary<Hex, GameObject> ();
		gameObjectToHexMap = new Dictionary<GameObject, Hex> ();

		for (int x = 0; x < mapWidth; x++)
		{
			for (int y = 0; y < mapHeight; y++) 
			{
				Hex h = new Hex(this, x, y );
				h.Elevation = -0.5f;
				hexes [x, y] = h;
				Vector3 pos = h.PosFromCamera (Camera.main.transform.position, mapWidth, mapHeight);

				GameObject hexGO = (GameObject)Instantiate (HexPrefab,
					h.Position(),
					Quaternion.identity,
					this.transform
				);
				hextoGameObjectMap [h] = hexGO;
				gameObjectToHexMap [hexGO] = h;
				h.TerrainType = Hex.TERRAIN_TYPE.OCEAN;
				h.ElevationType = Hex.ELEVATION_TYPE.WATER;
				hexGO.GetComponent<HexComponent> ().Hex = h;
				hexGO.GetComponent<HexComponent> ().HexMap = this;

				//This dtermines the material used to colour the tile
				MeshRenderer mr = hexGO.GetComponentInChildren<MeshRenderer> ();
				mr.material = MatOcean;

				MeshFilter mf = hexGO.GetComponentInChildren<MeshFilter> ();
				mf.mesh = MeshWater;
			}
		}
		UpdateHexVisuals ();
	}

	public void UpdateHexVisuals()
	{
		for (int column = 0; column < mapWidth; column++) 
		{
			Hex h = hexes[column,row];
			GameObject hexGO = hexToGameObjectMap[h];

			HexComponent hexComp = hexGO.GetComponentInChildren<HexComponent>();
			MeshRenderer mr = hexGO.GetComponentInChildren<MeshRenderer>();
			MeshFilter mf = hexGO.GetComponentInChildren<MeshFilter>();


			if(h.Elevation >= HeightFlat && h.Elevation < HeightMountain)
			{
				if(h.Moisture >= MoistureJungle)
				{
					mr.material = MatGrass;
					h.TerrainType = Hex.TERRAIN_TYPE.GRASSLANDS;
					h.FeatureType = Hex.FEATURE_TYPE.RAINFOREST;

					Vector3 p = hexGO.transform.position;
					if(h.Elevation >= HeightHill)
					{
						p.y += 0.25f;
					}


					GameObject.Instantiate(JunglePrefab, p, Quaternion.identity, hexGO.transform);
				}
				else if(h.Moisture >= MoistureForest)
				{
					mr.material = MatGrass;
					h.TerrainType = Hex.TERRAIN_TYPE.GRASSLANDS;
					h.FeatureType = Hex.FEATURE_TYPE.FOREST;

					Vector3 p = hexGO.transform.position;
					if(h.Elevation >= HeightHill)
					{
						p.y += 0.25f;
					}
					GameObject.Instantiate(ForestPrefab, p, Quaternion.identity, hexGO.transform);
				}
				else if(h.Moisture >= MoistureGrass)
				{
					mr.material = MatGrass;
					h.TerrainType = Hex.TERRAIN_TYPE.GRASSLANDS;
				}
				else if(h.Moisture >= MoisturePlains)
				{
					mr.material = MatPlains;
					h.TerrainType = Hex.TERRAIN_TYPE.PLAINS;
				}
				else 
				{
					mr.material = MatDesert;
					h.TerrainType = Hex.TERRAIN_TYPE.DESERT;
				}
		}
			if(h.Elevation >= HeightMountain)
			{
				mr.material = MatMountains;
				mf.mesh = MeshMountain;
				h.ElevationType = Hex.ELEVATION_TYPE.MOUNTAIN;
			}
			else if(h.Elevation >= HeightHill)
			{
				h.ElevationType = Hex.ELEVATION_TYPE.HILL;
				mf.mesh = MeshHill;
				hexComp.VerticalOffset = 0.25f;
			}
			else if(h.Elevation >= HeightFlat)
			{
				h.ElevationType = Hex.ELEVATION_TYPE.FLAT;
				mf.mesh = MeshFlat;
			}
			else
			{
				h.ElevationType = Hex.ELEVATION_TYPE.WATER;
				mr.material = MatOcean;
				mf.mesh = MeshWater;
			}
	}
}
	public Hex[] GetHexesWithinRangeOf(Hex centreHex, int range)
	{
		List<Hex> results = new List<Hex>();

		for (int dx = -range; dx < range-1; dx++)
		{
			for (int dy = Mathf.Max(-range+1, -dx-range); dy < Mathf.Min(range, -dx+range-1); dy++)
			{
				results.Add( GetHexAt(centreHex.A + dx, centreHex.B + dy) );
			}
		}

		return results.ToArray();
	}
	public void SpawnUnitAt( Unit unit, GameObject prefab, int x, int y )
	{
		if(unitToGameObjectMap == null)
		{
			unitToGameObjectMap = new Dictionary<Unit, GameObject>();
		}

		Hex ThisHex = GetHexAt(x, y);
		GameObject ThisHexGO = hexToGameObjectMap[ThisHex];
		unit.SetHex(ThisHex);

		GameObject unitGO = (GameObject)Instantiate(prefab, ThisHexGO.transform.position, Quaternion.identity, ThisHexGO.transform);
		unit.OnObjectMoved += unitGO.GetComponent<UnitMove>().OnUnitMoved;

		CurrentPlayer.AddUnit(unit);
		unit.OnObjectDestroyed += OnUnitDestroyed;
		unitToGameObjectMap.Add(unit, unitGO);
	}
	public void OnUnitDestroyed( MapObject mo )
	{
		GameObject go = unitToGameObjectMap[(Unit)mo];
		unitToGameObjectMap.Remove((Unit)mo);
		Destroy(go);
}
	public void SpawnCityAt(City city, GameObject prefab, int x, int y)
	{
		if (cityToGameObjectMap == null) 
		{
			cityToGameObjectMap = new Dictionary<City, GameObject> ();

		}
		Hex ThisHex = GetHexAt(x, y);
		GameObject ThisHexGO = hexToGameObjectMap[ThisHex];
		try
		{
			city.SetHex(ThisHex);
		}
		catch(UnityException exception)
		{
			return;
		}
		city.SetHex(ThisHex);
		GameObject CityGO = (GameObject)Instantiate(prefab, ThisHexGO.transform.position, Quaternion.identity, ThisHexGO.transform);
		CurrentPlayer.AddCity (city);
		cityToGameObjectMap.Add (city, CityGO);

		if (OnCityCreated != null) 
		{
			OnCityCreated (city, CityGO);
		}
	}
}