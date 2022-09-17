using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Player {

	public Player(string name)
	{
		PlayerName = name;
		units = new HashSet<Unit> ();
		cities = new HashSet<City> ();
	}

	public string PlayerName;
	private HashSet<Unit> units;
	private HashSet<City> cities;

	public Unit[] Units
	{
		get{ return units.ToArray (); }
	}
	public City[] Cities
	{
		get{ return cities.ToArray (); }
	}
	public void AddUnit(Unit u)
	{
		units.Add (u);
	}
	public void AddCity(City c)
	{
		cities.Add (c);
	}
}
