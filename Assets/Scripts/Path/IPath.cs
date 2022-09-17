using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
	public interface IPath {
		IPath[] Getneighbours ();
		float EntryCost (float currentCost, IPath Source, IPathUnit unit);
	}
}