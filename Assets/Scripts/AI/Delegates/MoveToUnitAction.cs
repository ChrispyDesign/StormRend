using System.Linq;
using System.Collections.Generic;
using BhaVE.Core;
using BhaVE.Delegates;
using BhaVE.Nodes;
using BhaVE.Variables;
using UnityEngine;

namespace StormRend.Bhaviours
{
    /// <summary>
    /// Stops the agent's behaviour tree
    /// </summary>
    [CreateAssetMenu(menuName = "StormRend/Delegates/Actions/MoveToUnit", fileName = "MoveToUnit")]
    public class MoveToUnitAction : BhaveAction
    {
        //This agent should already be in range of the target, but just in case
        [SerializeField] bool checkInRange = false;

        [SerializeField] BhaveUnitList targets;

        Unit unit;
        List<Tile> validMoves = new List<Tile>();

        public override void Initiate(BhaveAgent agent)
        {
            unit = agent.GetComponent<Unit>();
        }

        public override NodeState Execute(BhaveAgent agent)
        {
            ///Move to the target
            //Find the closest valid tile
            Dijkstra.Instance.FindValidMoves(
                Grid.GetNodeFromCoords(targets.value[0].m_coordinates), 
                1, 
                (unit is PlayerUnit) ? typeof(EnemyUnit) : typeof(PlayerUnit));
            validMoves = Dijkstra.Instance.m_validMoves;
            
            validMoves = validMoves.OrderBy(
                x => (Vector2Int.Distance(unit.m_coordinates, targets.value[0].m_coordinates))).ToList();

            unit.MoveTo(validMoves[0]);

            return NodeState.Success;
        }
    }
}
