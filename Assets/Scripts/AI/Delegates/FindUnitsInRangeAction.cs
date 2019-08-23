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
		public UnitType unitTypeToFind = UnitType.Player;
		public BhaveUnitList targets;

		[Tooltip("The number of turns to cast out in order find the range of this unit")]
		[SerializeField] uint turns = 1;

		//Privates
		Unit u; //The unit mono attached to this agent
		List<Tile> tilesToScan = new List<Tile>();
		private bool unitsHaveBeenFound = false;


		HashSet<Tile> noDuplicates = new HashSet<Tile>();



		public override void Initiate(BhaveAgent agent)
		{
			u = agent.GetComponent<Unit>();
		}

		public override void Begin()
		{
			//Resets
			unitsHaveBeenFound = false;
			targets.value.Clear();

			noDuplicates.Clear();
		}

		public override NodeState Execute(BhaveAgent agent)
		{
			//Find valid moves
			Dijkstra.Instance.FindValidMoves(
				u.GetTile(),
				u.GetMoveRange() * (int)turns,
				(u is EnemyUnit) ? typeof(EnemyUnit) : typeof(PlayerUnit));
			tilesToScan = Dijkstra.Instance.m_validMoves;

			///TEMPORARY
			// foreach (var t in tilesToScan)
			// {
			// 	noDuplicates.Add(t);
			// }
			// tilesToScan.Clear();
			// foreach (var nd in noDuplicates)
			// {
			// 	tilesToScan.Add(nd);
			// }

			//DB: draw the valid moves
			// foreach (var v in tilesToScan)
			// {
			// 	v.m_attackCover.SetActive(true);
			// }

			if (tilesToScan.Count <= 0) return NodeState.Failure;

			//Determine if specified unit is in range
			foreach (var t in tilesToScan)
			{
				var unitOnTop = t.GetUnitOnTop();
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
			// PrintList(targets.value);
			return (unitsHaveBeenFound) ? NodeState.Success : NodeState.Failure;
		}

		void PrintList(IEnumerable<object> list)
		{
			foreach (var t in list)
			{
				Debug.Log(t);
			}
		}
	}
}