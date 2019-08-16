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
        [SerializeField] BhaveUnitList targets;
   		[Tooltip("The number of turns to cast out in order find the range of this unit")]
        [SerializeField] uint turns = 1;

        //Privates
        Unit unit;
        List<Tile> validMoves = new List<Tile>();

        public override void Initiate(BhaveAgent agent)
        {
            unit = agent.GetComponent<Unit>();
        }

        public override NodeState Execute(BhaveAgent agent)
        {
            Vector2Int oldCoord;

            oldCoord = unit.m_coordinates;

            ///Move as close as possible to the target
            Dijkstra.Instance.FindValidMoves(
                Grid.CoordToTile(unit.m_coordinates), 
                unit.GetRange() * (int)turns,
                (unit is PlayerUnit) ? typeof(EnemyUnit) : typeof(PlayerUnit));
            validMoves = Dijkstra.Instance.m_validMoves;
            foreach (var vm in validMoves)
            {
                Debug.Log("Before Sort Distances: " + Vector2Int.Distance(unit.m_coordinates, vm.GetCoordinates()));
            }
            
            validMoves = validMoves.OrderBy(
                x => (Vector2Int.Distance(targets.value[0].m_coordinates, x.GetCoordinates()))).ToList();
            foreach (var vm in validMoves)
            {
                Debug.Log("After Sort Distances: " + Vector2Int.Distance(unit.m_coordinates, vm.GetCoordinates()));
            }

            unit.MoveTo(validMoves[0]);

            Debug.LogFormat("MoveToUnitAction: {0} > {1}", oldCoord, unit.m_coordinates);

            return NodeState.Success;
        }
    }
}
