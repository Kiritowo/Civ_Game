using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PathFinding
{
	public class AStar<T> where T : PathFinding
	{
		public AStar(IPathWorld world, IPathUnit unit, T Source, T Destination, CostEst CostEstFunction)
		{
			this.world = world;
			this.unit = unit;
			this.Source = Source;
			this.Destination = Destination;
			this.CostEstFunction = CostEstFunction;
		}
		IPathWorld world;
		IPathUnit unit;
		T Source;
		T Destination;
		CostEst CostEstFunction;

		Queue<T> path;

		public void Work()
		{
			path = new Queue<T> ();
			HashSet<T> closedSet = new HashSet<T> ();
			PathfindingPriorityQueue<T> openSet = new PathfindingPriorityQueue<T> ();
			openSet.Enqueue (Source, 0);
			Dictionary<T, T> From = new Dictionary<T, T> ();
			Dictionary<T, float> gCost = new Dictionary<T, float> ();
			gCost [Source] = 0;
			Dictionary<T, float> fCost = new Dictionary<T, float> ();
			fCost [Source] = CostEstFunction(Source, Destination);

			while (openSet.Count > 0) 
			{
				T current = openSet.Dequeue ();
				if (System.Object.ReferenceEquals(current, Destination)) 
				{
					Reconstruct_path (From, current);
					return;
				}
				closedSet.Add (current);
				foreach (T edge_neighbour in current.Getneighbours()) 
				{
					T neighbour = edge_neighbour;
					if (closedSet.Contains (neighbour)) 
					{
						continue;
					}
					float costToNeighbour = neighbour.EntryCost(gCost[current], current, unit);
					if (costToNeighbour < 0) 
					{
						continue;
					}
					float g2Cost = gCost [current] + costToNeighbour;
					if (openSet.Contains (neighbour) && g2Cost >= gCost [neighbour]) 
					{
						continue;
					}
					From [neighbour] = current;
					gCost [neighbour] = g2Cost;
					fCost [neighbour] = gCost [neighbour] + CostEstFunction (neighbour, Destination);
					openSet.EnqueueOrUpdate (neighbour, fCost [neighbour]);
				}
			}
		}
		float Est(T s, T d)
		{
			
		}
		public T[]GetList()
		{
			return path.ToArray ();
		}
	}
}