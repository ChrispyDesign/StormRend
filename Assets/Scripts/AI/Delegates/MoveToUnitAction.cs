using System.Linq;
using System.Collections.Generic;
using BhaVE.Core;
using BhaVE.Delegates;
using BhaVE.Nodes;
using BhaVE.Variables;
using UnityEngine;
using StormRend.Settings;

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
		Unit u;
		List<Tile> validMoves = new List<Tile>();

		public override NodeState Execute(BhaveAgent agent)
		{
			//If there aren't any targets then fail
			if (targets.value.Count <= 0) return NodeState.Failure;

			//Get this agent's unit
			u = agent.GetComponent<Unit>();

			//Find the valid moves
			validMoves = Dijkstra.Instance.GetValidMoves(
				u.GetTile(), 	//The tile the agent is current on
				u.GetMoveRange() * (int)turns,		//Scan move range by turns
				(u is EnemyUnit) ? typeof(EnemyUnit) : typeof(PlayerUnit));

			//Check to see if the target is already next to this agent before moving
			if (TargetIsAdjacent(false)) return NodeState.Success;

			//Move as close as possible to the target (targets.value[0] should be the closest unit)
			validMoves = validMoves.OrderBy(x => (Vector2Int.Distance(targets.value[0].coords, x.GetCoordinates()))).ToList();
			// Debug.Log("Before Sort"); for (int i = 0; i < validMoves.Count; i++) Debug.Log(Vector2Int.Distance(targets.value[0].coords, validMoves[i].GetCoordinates()));
			// Debug.Log("After Sort"); for (int i = 0; i < validMoves.Count; i++) Debug.Log(Vector2Int.Distance(targets.value[0].coords, validMoves[i].GetCoordinates()));

			//Move the agent
			if (validMoves.Count > 0)
			{
				for(int i = 0; i < validMoves.Count; i++)
				{
					if(validMoves[i].GetUnitOnTop() == null)
					{
						u.MoveTo(validMoves[i]);
						break;
					}
				}
			}

			//If target is next to opponent then successful chase
			if (TargetIsAdjacent(false))
				return NodeState.Success;
			else
				return NodeState.Failure;
		}

		//Checks if the opponent unit is on an adjacent tile
		bool TargetIsAdjacent(bool checkDiagonal)
		{
			float tileSize = GameSettings.singleton.tileSize;
			const float KadjDist = 1f;
			const float KdiagDist = 1.414213f;
			const float Ktolerance = 0.1f;
			Tile opponentTile = null;

			//Find opponent
			foreach (var m in validMoves)
			{
				if (m.GetUnitOnTop() == targets.value[0])
				{
					//Closest opponent found
					opponentTile = m;
					break;
				}
			}

			//Check for adjacency
			if (opponentTile)
			{
				float dist = Vector3.Distance(u.transform.position, opponentTile.transform.position);

				//Check for diagonal adjacency first
				if (checkDiagonal)
				{
					if ((dist - (KdiagDist * tileSize - Ktolerance)) * ((KdiagDist * tileSize + Ktolerance) - dist) >= 0)
						return true;
				}
				if ((dist - (KadjDist * tileSize - Ktolerance)) * ((KadjDist * tileSize + Ktolerance) - dist) >= 0)
					return true;
			}
			return false;
		}

		bool TargetIsAdjacent(bool checkDiagonal, Tile opponentTile)
		{
			float tileSize = GameSettings.singleton.tileSize;
			const float KadjDist = 1f;
			const float KdiagDist = 1.414213f;
			const float Ktolerance = 0.1f;

			//Check for adjacency
			if (opponentTile)
			{
				float dist = Vector3.Distance(u.transform.position, opponentTile.transform.position);

				//Check for diagonal adjacency first
				if (checkDiagonal)
				{
					if ((dist - (KdiagDist * tileSize - Ktolerance)) * ((KdiagDist * tileSize + Ktolerance) - dist) >= 0)
						return true;
				}
				if ((dist - (KadjDist * tileSize - Ktolerance)) * ((KadjDist * tileSize + Ktolerance) - dist) >= 0)
					return true;
			}
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
