using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Pathfinding
{

	public interface IPathUnit{
		float EntryCost (IPath Source, IPath Destination);
	}
}