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
        public UnitType unitType;
        public BhaveUnitList targets;

        //Privates
        Unit unit;	//The unit mono attached to this agent
        List<Tile> validMoves = new List<Tile>();
        private bool unitsHasBeenFound = false;

        public override void Initiate(BhaveAgent agent)
        {
            unit = agent.GetComponent<Unit>();
        }

        public override void Begin()
        {
			//Resets
            unitsHasBeenFound = false;
			targets.value.Clear();
        }

        public override NodeState Execute(BhaveAgent agent)
        {
            //Find valid moves
            Dijkstra.Instance.FindValidMoves(Grid.GetNodeFromCoords(unit.m_coordinates), unit.GetMove(), typeof(EnemyUnit));
            validMoves = Dijkstra.Instance.m_validMoves;

            //Determine if specified unit is in range
            foreach (var n in validMoves)
            {
                var unitOnTop = n.GetUnitOnTop();
                if (unitType == UnitType.Player && unitOnTop is PlayerUnit)
                {
					//Update targets
                    targets.value.Add(unitOnTop);	
                    unitsHasBeenFound = true;
                }
                else if (unitType == UnitType.Enemy && unitOnTop is EnemyUnit)
                {
                    targets.value.Add(unitOnTop);
					unitsHasBeenFound = true;
                    return NodeState.Success;
                }
            }
            return (unitsHasBeenFound) ? NodeState.Success : NodeState.Failure;
        }

    }
}