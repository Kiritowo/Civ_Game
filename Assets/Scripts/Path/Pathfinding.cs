using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Pathfinding
{
	
public class Pathfinding {

		public static T[] FindPath<T>(IPathWorld world, IPathUnit unit, T Source, T Destination, CostEst CostEstFunction) where T : IPath
		{
			if (world == null || unit == null || Source == null || Destination == null) 
			{
				return null;
			}
			AStar<T> resolver = new AStar<T>(world, unit, Source, Destination, CostEstFunction);
			resolver.Turn ();

			return resolver.GetList();
		}
	}

	public delegate float CostEst(IPath a, IPath b);
}