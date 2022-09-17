using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMap_continent : HexMap {

	override public void GenerateMap()
	{
		base.GenerateMap ();

		int continentNum = 4;
		int spacing = mapWidth / continentNum;

		Random.InitState (0);
		for (int i = 0; i < continentNum; i++) 
		{
			int RaisedNum = Random.Range (4, 8);
			for (int j = 0; j < RaisedNum; j++) 
			{
				int range = Random.Range (5,8);
				int d = Random.Range (range, mapHeight - range);
				int e = Random.Range (0, 10) - d / 2 + (i * spacing);
				Elevation (d, e, range);
			}
		}

		float noiseResolution = 0.01f;
		Vector2 noiseOffset = new Vector2( Random.Range(0f, 1f), Random.Range(0f, 1f) ); 
		float noiseScale = 2f;

		for (int column = 0; column < mapWidth; column++)
			{
			for (int row = 0; row < mapHeight; row++)
				{
					Hex h = GetHexAt(column, row);
					float n = 
					Mathf.PerlinNoise( ((float)column/Mathf.Max(mapWidth,mapHeight) / noiseResolution) + noiseOffset.x, 
						((float)row/Mathf.Max(mapWidth,mapHeight) / noiseResolution) + noiseOffset.y )
						- 0.5f;
				h.Elevation += n * noiseScale;
				}
			}
		noiseResolution = 0.05f;
		noiseOffset = new Vector2( Random.Range(0f, 1f), Random.Range(0f, 1f) );
		noiseScale = 2f;

		for (int column = 0; column < mapWidth; column++)
		{
			for (int row = 0; row < mapHeight; row++)
			{
				Hex h = GetHexAt(column, row);
				float n = 
					Mathf.PerlinNoise( ((float)column/Mathf.MaxcolumnNum,rowNum) / noiseResolution) + noiseOffset.x, 
						((float)row/Mathf.Max(columnNum,rowNum) / noiseResolution) + noiseOffset.y )
					- 0.5f;
				h.Moisture = n * noiseScale;
			}
		}

		UpdateHexVisuals();

		Unit unit = new Unit();

		unit.CityBuilder = true;

		SpawnUnitAt(unit, UnitKnightPrefab, 36, 15);

		City city = new City();
		SpawnCityAt(city, CityPrefab, 36, 14);
	}
	void ElevateArea(int x, int y, int range, float centreHeight = .8f)
	{
		Hex centreHex = GetHexAt(x, y);

		Hex[] areaHexes = GetHexesWithinRangeOf(centreHex, range);

		foreach(Hex h in areaHexes)
		{
			h.Elevation = centreHeight * Mathf.Lerp( 1f, 0.25f, Mathf.Pow(Hex.Distance(centreHex, h) / range,2f) );
		}
	}
	}
