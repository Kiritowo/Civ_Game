using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PathFinding
{
	public class IPathGraph
	{
		Dictionary<IPathGraph, IPathGraph[]> neighbours;

		public IPathGraph(IPathWorld world)
		{
		}
	}
}