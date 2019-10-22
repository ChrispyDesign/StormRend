using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using pokoro.BhaVE.Core.Delegates;
using pokoro.BhaVE.Core.Enums;
using pokoro.BhaVE.Core;
using StormRend.Variables;
using StormRend.MapSystems.Tiles;
using StormRend.Units;
using StormRend.MapSystems;

namespace StormRend.Bhaviours
{
	/// <summary>
	/// Stops the agent's behaviour tree
	/// </summary>
	[CreateAssetMenu(menuName = "StormRend/Delegates/Actions/MoveToUnit", fileName = "MoveToUnit")]
    public class MoveToUnitAction : BhaveAction
    {
        [SerializeField] UnitListVar targets = null;
        [Tooltip("The number of turns to cast out in order find the range of this unit")]
        [SerializeField] int turns = 1;

        //Members
        AnimateUnit au;
        // List<Tile> validMoves = new List<Tile>();
		Tile[] validMoves;

	#region Core
        public override NodeState Execute(BhaveAgent agent)
        {
        	//If there aren't any targets then fail
        	if (targets.value.Count <= 0) return NodeState.Failure;

        	//Get this agent's unit
        	au = agent.GetComponent<AnimateUnit>();

        	//Find the valid moves
			validMoves = Map.CalculateTileRange(au.currentTile.owner, au.currentTile, turns, au.GetType());	//Just ignore the same unit type as this unit

        	//Check to see if the target is already next to this agent before moving
        	if (TargetIsAdjacent()) return NodeState.Success;

            //Move as close as possible to the target (targets.value[0] should be the closest unit)
            validMoves = validMoves.OrderBy(x => (Vector3.Distance(targets.value[0].currentTile.transform.position, x.transform.position))).ToArray();
            // validMoves = validMoves.OrderBy(x => (Vector2Int.Distance(targets.value[0].coords, x.GetCoordinates()))).ToList();

        	//Move the agent
        	if (validMoves.Length > 0)
        		au.Move(validMoves[1]);

        	//If target is next to opponent then successful chase
        	if (TargetIsAdjacent())
        		return NodeState.Success;
        	else
        		return NodeState.Pending;
        }

        bool TargetIsAdjacent()
        {
			//TODO This needs to be rewritten!!!

        	// if (validMoves.Count < 4) return false;

        	// for (int i = 0; i < 4; ++i)
        	// 	if (validMoves[i].GetUnitOnTop() == targets.value[0])
        	// 		return true;

        	return false;
        }
	#endregion

		//Utility
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
