using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MapObject {

	public MapObject()
	{
		
	}
	public string Name;
	public int HitPoints = 100;
	public bool Attackable = true;
	public int FactionID= 0;

	public Hex Hex { get; protected set;}

	public delegate void OnObjectMoved(Hex oldHex, Hex newHex);
	public event OnObjectMoved UnitMoved;

	virtual public void SetHex(Hex newHex)
	{
		Hex oldHex = Hex;
		Hex = newHex;
		if (UnitMoved != null) 
		{
			UnitMoved (oldHex, newHex);
		}
	}

}
