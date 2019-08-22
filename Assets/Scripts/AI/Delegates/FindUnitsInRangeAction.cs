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
		List<Tile> validMoves = new List<Tile>();
		private bool unitsHaveBeenFound = false;

		public override void Initiate(BhaveAgent agent)
		{
			u = agent.GetComponent<Unit>();
		}

		public override void Begin()
		{
			//Resets
			unitsHaveBeenFound = false;
			targets.value.Clear();

			PrintList(targets.value);
			Debug.Break();
		}

		public override NodeState Execute(BhaveAgent agent)
		{
			//Find valid moves
			Dijkstra.Instance.FindValidMoves(
				u.GetTile(),
				u.GetMoveRange() * (int)turns,
				(u is EnemyUnit) ? typeof(EnemyUnit) : typeof(PlayerUnit));
			validMoves = Dijkstra.Instance.m_validMoves;

			Debug.Log("Validmoves Count: " + validMoves.Count);
			PrintList(validMoves);
			Debug.Break();

			if (validMoves.Count <= 0) return NodeState.Failure;

			//Determine if specified unit is in range
			foreach (var t in validMoves)
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
			Debug.Log("FindUnitsInRange: UnitsHaveBeenFound: " + unitsHaveBeenFound);
			Debug.Break();
			return (unitsHaveBeenFound) ? NodeState.Success : NodeState.Failure;
		}

		void PrintList(IEnumerable<object> list)
		{
			Debug.Log("List contents: ");
			foreach (var u in list)
			{
				Debug.Log(u);
			}
		}
	}
}