using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class City : MapObject {

	public City()
	{
		Name = "Lothric";
		TEST ();
	}

	BuildingJob buildingJob;

	float productionPerTurn = 77777;

	override public void SetHex(Hex newHex)
	{
		if (Hex != null) 
		{
			Hex.RemoveCity(this);
		}
		base.SetHex (newHex);
		Hex.AddCity (this);
	}

	public void EndTurn()
	{
		if (buildingJob != null) 
		{
			float LabourLeft = buildingJob.DoWork (productionPerTurn);
			if (LabourLeft <= 0) 
			{
				buildingJob = null;
			}
		}
	}

	void TEST()
	{
		buildingJob = new BuildingJob (
			null,
			"Knight",
			100,
			0,
			() => {
					this.Hex.HexMap.SpawnUnitAt(
						new Unit(),
					this.Hex.HexMap.UnitKnightPrefab,
						this.Hex.A,
						this.Hex.B
				);
			},
				null
		);
		}
	}
