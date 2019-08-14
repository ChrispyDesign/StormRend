using System.Collections.Generic;
using BhaVE.Core;
using BhaVE.Delegates;
using BhaVE.Nodes;
using UnityEngine;

namespace StormRend.Bhaviours
{
    /// <summary>
    /// Stops the agent's behaviour tree
    /// </summary>
    // [CreateAssetMenu(menuName = "StormRend/Delegates/Actions/MoveToUnit", fileName = "MoveToUnit")]
	public class MoveToUnitAction : BhaveAction
    {

		// List<Tile> validMoves = new List<Tile>

        public override NodeState Execute(BhaveAgent agent)
        {
			//Get valid moves
// Dijkstra.Instance.FindValidMoves(thisUnit.)
			//Fi

            throw new System.NotImplementedException();
        }
    }
}
