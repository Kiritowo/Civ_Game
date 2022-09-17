using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Unit : MapObject, IPathUnit {

	public Unit()
	{
		Name = "Knight";
	}
	public int attack = 8;
	public int Allowance = 2;
	public int AllowanceLeft = 2;
	public bool CityBuilder = false;

	List<Hex> Path;
	const bool rules = false;

	public void ClearPath()
	{
		this.Path = new List<Hex> ();
	}
	public void setPath(Hex[] Path)
	{
		this.Path = new List<Hex> (Path);
	}
	public Hex[] GetHexPath()
	{
		return (this.Path == null)? null : this.Path.ToArray ();
	}

	public bool UnitWaitOrders()
	{
		if (AllowanceLeft > 0 && (Path == null || Path.Count == 0)) 
		{
			return true;
		}
		return false;
	}

	public void RefillAllowance()
	{
		AllowanceLeft = Allowance;
	}

	public bool Turn()
	{
		if (AllowanceLeft <= 0)
			return false;
		if(Path == null || Path.Count == 0)
		{
			return false;
		}

		Hex oldHex = Path[0];
		Hex newHex = Path[1];

		int CostToEnter = MovementCost (newHex);
		if (CostToEnter > AllowanceLeft && AllowanceLeft < Allowance) 
		{
			return false;
		}
		Path.RemoveAt (0);
		if(Path.Count == 1)
		{
			Path = null;
		}

		SetHex (newHex);
		AllowanceLeft = Mathf.Max (AllowanceLeft-CostToEnter, 0);
		return Path != null && AllowanceLeft > 0;
	}

	public int MovementCost(Hex hex)
	{
		return hex.BaseMovementCost (false, false, false);
	}

	public float TurnsToHex(Hex hex, float turnsSoFar)
	{
		float baseMovementCost = MovementCost (hex) / Allowance;
		if (TurnsToHex < 0) 
		{
			return -1;
		}

		if (TurnsToHex > 1) 
		{
			TurnsToHex = 1;
		}

		float Remain = AllowanceLeft / Allowance;
		float turnsFull = Mathf.Floor (turnsSoFar);
		float turnsFraction = turnsSoFar - turnsFull;

		if (turnsFraction < 0.01f || turnsFraction > 0.99f) 
		{
			if(turnsFraction < 0.01f)
				turnsFraction = 0;
			if (turnsFraction > 0.99f) 
			{
				turnsFull += 1;
				turnsFraction = 0;
			}
		}

		float turnsUsed = turnsFraction + TurnsToHex;
		{
			if (rules) {
				if (turnsSoFar == 0) {
					
				} 
				else 
				{
					turnsFull += 1;
					turnsFraction = 0;
				}

				turnsUsed = TurnsToHex;
			} 
			else 
			{
				turnsUsed = 1;
			}
		}

		return turnsFull + turnsUsed;

	}

	override public void SetHex(Hex newHex)
	{
		if (Hex != null) 
		{
			Hex.RemoveUnit (this);
		}
		base.SetHex (newHex);

		Hex.AddUnit (this);
	}

	public float EntryCost(IPath Source, IPath Destination)
	{
		return 1;
	}

	public IPath[] GetNeighbours ()
	{
		throw new System.NotImplementedException ();
	}
	public float EntryCost(float currentCost, IPath Source, IPath unit)
	{
		throw new System.NotImplementedException ();
	}
}
