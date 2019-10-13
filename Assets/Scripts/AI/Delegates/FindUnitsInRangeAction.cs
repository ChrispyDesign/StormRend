using UnityEngine;
using System.Collections.Generic;
using StormRend.Defunct;
using pokoro.BhaVE.Core.Delegates;
using pokoro.BhaVE.Core.Enums;
using pokoro.BhaVE.Core;
using StormRend.Variables;

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
		public UnitType unitTypeToFind = UnitType.Player;
		public UnitListVar targets;

		[Tooltip("The number of turns to cast out in order find the range of this unit")]
		[SerializeField] uint turns = 1;

		//Privates
		xUnit u; //The unit mono attached to this agent
		List<xTile> tilesToScan = new List<xTile>();
		private bool unitsHaveBeenFound = false;


		public override void Begin()
		{
			//Resets
			unitsHaveBeenFound = false;
			targets.value.Clear();
		}

		public override NodeState Execute(BhaveAgent agent)
		{
			//Until each delegate object is deep copied, the current unit must be updated each tick
			u = agent.GetComponent<xUnit>();

			//Find valid moves
			xDijkstra.Instance.FindValidMoves(
				u.GetTile(),
				u.GetMoveRange() * (int)turns,
				(u is xEnemyUnit) ? typeof(xEnemyUnit) : typeof(xPlayerUnit));
			tilesToScan = xDijkstra.Instance.m_validMoves;

			if (tilesToScan.Count <= 0) return NodeState.Failure;

			//Determine if specified unit is in range
			foreach (var t in tilesToScan)
			{
				var unitOnTop = t.GetUnitOnTop();
				if (unitTypeToFind == UnitType.Player && unitOnTop is xPlayerUnit)
				{
					//Update targets
					targets.value.Add(unitOnTop);
					unitsHaveBeenFound = true;
				}
				else if (unitTypeToFind == UnitType.Enemy && unitOnTop is xEnemyUnit)
				{
					targets.value.Add(unitOnTop);
					unitsHaveBeenFound = true;
				}
			}
			// PrintList(targets.value);
			return (unitsHaveBeenFound) ? NodeState.Success : NodeState.Failure;
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