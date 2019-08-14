using BhaVE.Core;
using BhaVE.Delegates;
using BhaVE.Nodes;
using UnityEngine;
using StormRend;
using System.Collections.Generic;
using System;
using BhaVE.Variables;

namespace StormRend.Bhaviours
{
    /// <summary>
    /// 
    /// </summary>
    [CreateAssetMenu(menuName = "StormRend/BhaVE/Conditions/Units In Range")]
    [Serializable]
    public class UnitsInRangeCondition : BhaveCondition
    {
        public enum UnitType { Player, Enemy }
        public UnitType unitType;
        public BhaveUnitList targets;

        //Privates
        Unit thisUnit;
        List<Node> validMoves = new List<Node>();
        private bool unitFound;
        private bool unitHasBeenFound = false;

        public override void Initiate(BhaveAgent agent)
        {
            thisUnit = agent.GetComponent<Unit>();
        }

        public override void Begin()
        {
			//Resets
            unitHasBeenFound = false;
			targets.value.Clear();
        }

        public override NodeState Execute(BhaveAgent agent)
        {
            //Find valid moves
            Dijkstra.Instance.FindValidMoves(Grid.GetNodeFromCoords(thisUnit.m_coordinates), thisUnit.GetMove(), typeof(EnemyUnit));
            validMoves = Dijkstra.Instance.m_validMoves;

            //Determine if specified unit is in range
            foreach (var n in validMoves)
            {
                var unitOnTop = n.GetUnitOnTop();
                if (unitType == UnitType.Player && unitOnTop is PlayerUnit)
                {
					//Update targets
                    targets.value.Add(unitOnTop);	
                    unitHasBeenFound = true;
                }
                else if (unitType == UnitType.Enemy && unitOnTop is EnemyUnit)
                {
                    targets.value.Add(unitOnTop);
                    return NodeState.Success;
                }
            }
            return (unitHasBeenFound) ? NodeState.Success : NodeState.Failure;
        }

    }
}