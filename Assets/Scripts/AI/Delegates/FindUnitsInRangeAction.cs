using BhaVE.Core;
using BhaVE.Delegates;
using BhaVE.Nodes;
using UnityEngine;
using System.Collections.Generic;
using BhaVE.Variables;

namespace StormRend.Bhaviours
{
	/// <summary>
	/// Finds a certain unit within range of agent and then adds it to a list (BhaveVar)
	/// Returns Success if units are found, Failure if none found
	/// </summary>
	[CreateAssetMenu(menuName = "StormRend/Delegates/Actions/FindUnitsInRange", fileName = "FindUnitsInRange")]
    public class FindUnitsInRangeAction : BhaveAction
    {
        public enum UnitType { Player, Enemy }
        public UnitType unitTypeToFind;
        public BhaveUnitList targets;

		[Tooltip("The number of turns to cast out in order find the range of this unit")]
		[SerializeField] uint turns = 1;	

        //Privates
        Unit unit;	//The unit mono attached to this agent
        List<Tile> validMoves = new List<Tile>();
        private bool unitsHaveBeenFound = false;

        public override void Initiate(BhaveAgent agent)
        {
            unit = agent.GetComponent<Unit>();
        }

        public override void Begin()
        {
			//Resets
            unitsHaveBeenFound = false;
			targets.value.Clear();
        }

        public override NodeState Execute(BhaveAgent agent)
        {
            //Find valid moves
            Dijkstra.Instance.FindValidMoves(
                Grid.CoordToTile(unit.m_coordinates), 
                unit.GetRange() * (int)turns, 
                (unit is EnemyUnit) ? typeof(EnemyUnit) : typeof(PlayerUnit));
            validMoves = Dijkstra.Instance.m_validMoves;
 
            Debug.Log("Validmoves Count: " + validMoves.Count);
            if (validMoves.Count <= 0) return NodeState.Failure;

            //Determine if specified unit is in range
            foreach (var n in validMoves)
            {
                var unitOnTop = n.GetUnitOnTop();
                if (unitTypeToFind == UnitType.Player && unitOnTop is PlayerUnit)
                {
					//Update targets
                    targets.value.Add(unitOnTop);	
                    unitsHaveBeenFound = true;
                }
                else if (unitTypeToFind == UnitType.Enemy && unitOnTop is EnemyUnit)
                {
                    targets.value.Add(unitOnTop);
					unitsHaveBeenFound = true;
                }
            }
            Debug.Log("FindUnitsInRange: UnitsHaveBeenFound: " + unitsHaveBeenFound);
            Debug.Break();
            return (unitsHaveBeenFound) ? NodeState.Success : NodeState.Failure;
        }

    }
}