using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using StormRend.Defunct;
using pokoro.BhaVE.Core.Delegates;
using pokoro.BhaVE.Core.Enums;
using pokoro.BhaVE.Core;
using StormRend.Variables;

namespace StormRend.Bhaviours
{
    /// <summary>
    /// Stops the agent's behaviour tree
    /// </summary>
    [CreateAssetMenu(menuName = "StormRend/Delegates/Actions/MoveToUnit", fileName = "MoveToUnit")]
    public class MoveToUnitAction : BhaveAction
    {
        [SerializeField] UnitListVar targets;
   		[Tooltip("The number of turns to cast out in order find the range of this unit")]
        [SerializeField] uint turns = 1;

        //Privates
        xUnit u;
        List<xTile> validMoves = new List<xTile>();

        public override NodeState Execute(BhaveAgent agent)
		{
			//If there aren't any targets then fail
			if (targets.value.Count <= 0) return NodeState.Failure;

			//Get this agent's unit
			u = agent.GetComponent<xUnit>();

			//Find the valid moves
            xDijkstra.Instance.FindValidMoves(
				u.GetTile(), 	//The tile the agent is current on
                u.GetMoveRange() * (int)turns,		//Scan move range by turns
                (u is xEnemyUnit) ? typeof(xEnemyUnit) : typeof(xPlayerUnit));
            validMoves = xDijkstra.Instance.m_validMoves;

			//Check to see if the target is already next to this agent before moving
			if (TargetIsAdjacent()) return NodeState.Success;

            //Move as close as possible to the target (targets.value[0] should be the closest unit)
			// Debug.Log("Before Sort"); for (int i = 0; i < validMoves.Count; i++) Debug.Log(Vector2Int.Distance(targets.value[0].coords, validMoves[i].GetCoordinates()));
            validMoves = validMoves.OrderBy(x => (Vector2Int.Distance(targets.value[0].coords, x.GetCoordinates()))).ToList();
			// Debug.Log("After Sort"); for (int i = 0; i < validMoves.Count; i++) Debug.Log(Vector2Int.Distance(targets.value[0].coords, validMoves[i].GetCoordinates()));

			//Move the agent
			if (validMoves.Count > 0)
				u.MoveTo(validMoves[1]);

			//If target is next to opponent then successful chase
			if (TargetIsAdjacent())
				return NodeState.Success;
			else
				return NodeState.Pending;
        }

		bool TargetIsAdjacent()
		{
			if (validMoves.Count < 4) return false;

			for (int i = 0; i < 4; ++i)
				if (validMoves[i].GetUnitOnTop() == targets.value[0])
					return true;

			return false;
		}

		void PrintList(IEnumerable<object> list)
		{
			Debug.LogFormat("{0}.{1}:", this.GetType().Name, list.GetType().Name);
			foreach (var t in list)
			{
				Debug.Log(t);
			}
		}
    }
}
