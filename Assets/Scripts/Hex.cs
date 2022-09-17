using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using PathFinding;

public class Hex : IPath {
	//This section is used to set the variables for the maths funtions
	public readonly int A;
	public readonly int B;
	public readonly int C;

	public Hex(int a, int b)
	{
		this.A = a;
		this.B = b;
		this.C = -(a + b);
		units = new HashSet<Unit> ();

	}
	//

	public float Elevation;
	public float Moisture;

	public enum TERRAIN_TYPE { PLAINS, GRASSLANDS, MARSH, FLOODPLAINS, DESERT, LAKE, OCEAN }
	public enum ELEVATION_TYPE { FLAT, HILL, MOUNTAIN, WATER }

	public TERRAIN_TYPE TerrainType { get; set; }
	public ELEVATION_TYPE ElevationType { get; set; }

	public enum FEATURE_TYPE { NONE, FOREST, RAINFOREST, MARSH }
	public FEATURE_TYPE FeatureType { get; set; }
	//
	public int MoveCost(bool HillScout, bool ForestScout, bool Flyable)
	{
		if ((ElevationType == ELEVATION_TYPE.MOUNTAIN || ElevationType == ELEVATION_TYPE.WATER) && Flyable == false)
			return -1;
		int movementcost = 1;
		if (ElevationType == ELEVATION_TYPE.HILL && HillScout == false && Flyable == false) 
			movementcost += 1;
		if (FeatureType == FEATURE_TYPE.FOREST || FeatureType == FEATURE_TYPE.RAINFOREST && ForestScout == false)
			movementcost += 1;
	}
	//
	public readonly HexMap HexMap;
	//This calculates the width of the hexagon
	static readonly float WidthM = Mathf.Sqrt (3) / 2;
	//This sets the radius for the hexagon
	float radius = 1f;

	HashSet<Unit> units;
	City city;
	public Unit[] Units { 
		get{
			return units.ToArray();
		}
	}
	public City City { get; protected set; }

	public override string ToString()
	{
		return A + ", " + B;
	}

	//This tells the camera what kind of wrap around is allowed
	public bool allowWrapEastWest = true;
	public bool allowWrapNorthSouth = false;

	//This section calculated the height and width of
	//the hexagons as well as their spacing
	public Vector3 Position()
	{
		float horizontal = HexWidth();
		float vertical = HexHeight() * 0.75f;

		return new Vector3 (
			horizontal * (this.A + this.B/2f),
			0,
			vertical * this.B
		);
	}
	public float HexHeight()
	{
		return radius * 2;
	}

	public float HexWidth()
	{
		return WidthM * HexHeight ();
	}

	public float HexVertSpacing ()
	{
		return HexHeight () * 0.75f;
	}

	public float HexHorizSpace()
	{
		return HexWidth();
	}

	public Vector3 PosFromCamera()
	{
		return HexMap.GetHexPosition (this);
	}
	//Determines camera position and allows it to loop round
	public Vector3 PosFromCamera (Vector3 cameraPosition, float rowNum, float columnNum)
	{
		float mapHeight = rowNum *  HexVertSpacing();
		float mapWidth = columnNum * HexHorizSpace();

		Vector3 position = Position();

		if (allowWrapEastWest) 
		{
			float mapWidthFromCamera = (position.x - cameraPosition.x) / mapWidth;
			if (Mathf.Abs (mapWidthFromCamera) <= 0.5f) {
				return position;
			}

			if (mapWidthFromCamera > 0)
				mapWidthFromCamera += 0.5f;
			else
				mapWidthFromCamera -= 0.5f;

			int widthFix = (int)mapWidthFromCamera;

			position.x -= widthFix * mapWidth;
		}
		//While this section isn't used it is necessary for the script to compile
		if (allowWrapNorthSouth) 
		{
			float mapHeightFromCamera = (position.z - cameraPosition.z) / mapHeight;
			if (Mathf.Abs (mapHeightFromCamera) <= 0.5f) {
				return position;
			}

			if (mapHeightFromCamera > 0)
				mapHeightFromCamera += 0.5f;
			else
				mapHeightFromCamera -= 0.5f;

			int heightFix = (int)mapHeightFromCamera;

			position.z -= heightFix * mapHeight;
		}

		return position;
	}
	//

	public static float CostEst(IPath xx, IPath yy)
	{
		return Distance ((Hex)xx, (Hex)yy);
	}
	//

	public static float Distance(Hex o, Hex p)
	{
		// WARNING: Probably Wrong for wrapping
		int dQ = Mathf.Abs(o.A - p.A);
		if(o.HexMap.allowWrapEastWest)
		{
			if(dQ > o.HexMap.mapWidth / 2)
				dQ = o.HexMap.mapWidth - dQ;
		}

		int dR = Mathf.Abs(o.B - p.B);
		if(o.HexMap.allowWrapNorthSouth)
		{
			if(dR > o.HexMap.mapHeight / 2)
				dR = o.HexMap.mapHeight - dR;
		}

		return 
			Mathf.Max( 
				dQ,
				dR,
				Mathf.Abs(o.C - p.C)
			);
	}
	//

	public void AddUnit( Unit unit)
	{
		if (units == null) {
			units = new HashSet<Unit> ();
		}
		units.Add(unit);
	}

	public void RemoveUnit(Unit unit)
	{
		if (units != null) 
		{
			units.Remove (unit);
		}
	}

	public void AddCity(City city)
	{
		if (city != null) 
		{
			throw new UnityException ("Don't do that");
			return;
		}
		this.city = city;
	}

	public void RemoveCity(City city)
	{
		if (city == null) 
		{
			return;
		}
		if (city != city) 
		{
			return;
		}
		this.city = null;
	}

	public int BaseMovementCost()
	{
		return MoveCost;
	}

	Hex[] neighbours;
	public IPath[] GetNeighbours()
	{
		if (this.neighbours != null)
			return this.neighbours;
		List<Hex> neighbours = new List<Hex>();
		neighbours.Add (HexMap.GetHexAt (A + +1,  B + 0));
		neighbours.Add (HexMap.GetHexAt (A + -1,  B + 0));
		neighbours.Add (HexMap.GetHexAt (A +  0,  B + +1));
		neighbours.Add (HexMap.GetHexAt (A +  0,  B + -1));
		neighbours.Add (HexMap.GetHexAt (A + +1,  B + -1));
		neighbours.Add (HexMap.GetHexAt (A + -1,  B + +1));

		List<Hex> neighboursB = new List<Hex> ();
		foreach (Hex h in neighbours) 
		{
			if (h != null) 
			{
				neighboursB.Add (h);
			}
		}
		this.neighbours = neighboursB.ToArray ();
		return this.neighbours;
	} 

	public float EntryCost(float currentCost, IPath Source, IPath theUnit)
	{
		return ((Unit)theUnit).EntryCost (this, currentCost);
	}

}
