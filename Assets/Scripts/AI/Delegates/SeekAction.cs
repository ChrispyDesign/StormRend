using System.Linq;
using UnityEngine;
using pokoro.BhaVE.Core.Delegates;
using pokoro.BhaVE.Core.Enums;
using pokoro.BhaVE.Core;
using StormRend.Variables;
using StormRend.Units;
using StormRend.MapSystems;
using StormRend.Utility;

namespace StormRend.Bhaviours
{
    /// <summary>
    /// Moves toward the closest target unit
    /// </summary>
    [CreateAssetMenu(menuName = "StormRend/AI/SeekAction", fileName = "SeekAction")]
	public sealed class SeekAction : BhaveAction
	{
		[SerializeField] UnitListVar targets = null;

		[Tooltip("The seeking range in terms of no. of turns")]
		[SerializeField] int rangeInTurns = 1;

		//Members
		UnitRegistry ur;
		AnimateUnit au;
		// Tile[] validMoves;

		#region Core
		public override void Initiate(BhaveAgent agent)
		{
			ur = UnitRegistry.current;
			//Get this agent's unit
			au = agent.GetComponent<AnimateUnit>();
		}

		public override void Begin()
		{
			targets.value.Clear();
		}

		public override NodeState Execute(BhaveAgent agent)
		{
			//Populate move tiles
			au.CalculateMoveTiles();

			//Populate targets. Check to see if opponent is within range (for enemies their CalculateMoves should've been run at the start of the player's turn)
			var opponentUnits = ur.GetUnitsByType<AllyUnit>();
			Debug.Log("--------- Populating list of allies ---------");
			opponentUnits.Print();

			//Within range means it is standing in this agent's move tile
			foreach (var o in opponentUnits)
				if (au.possibleMoveTiles.Contains(o.currentTile))
					targets.value.Add(o);
			Debug.Log("--------- Get targets in range ------------");
			targets.value.Print();

			//Check if unit is already adjacent
			if (TargetIsAdjacent()) return NodeState.Success;
			Debug.Log("--------- No targets adjacent -----------");

			//Check if any target is provoking
			foreach (var t in targets.value)
			{
				var at = t as AnimateUnit;
				if (at.isProvoking)
				{
					targets.value.Clear();      //Set as the main target
					targets.value.Add(t);
					Debug.Log("--------- Found a provoke target -----------");
					break;
				}
			}

			//Move toward
			//Get the closest tile to the nearest enemy
			var closestTile = au.possibleMoveTiles.OrderBy(x => Vector3.Distance(x.transform.position, targets.value[0].currentTile.transform.position)).ElementAt(0);
			au.Move(closestTile);

			//Move as close as possible to the nearest target
			//Find the valid moves
			// validMoves = Map.GetPossibleTiles(au.currentTile.owner, au.currentTile, rangeInTurns, au.GetType());	//Just ignore the same unit type as this unit
			//Check to see if the target is already next to this agent before moving
			// if (TargetIsAdjacent()) return NodeState.Success;
			//Move as close as possible to the target (targets.value[0] should be the closest unit)
			// validMoves = validMoves.OrderBy(x => (Vector3.Distance(targets.value[0].currentTile.transform.position, x.transform.position))).ToArray();
			// validMoves = validMoves.OrderBy(x => (Vector2Int.Distance(targets.value[0].coords, x.GetCoordinates()))).ToList();

			//Move the agent
			// if (validMoves.Length > 0)
			// au.Move(validMoves[1]);

			//If target is next to opponent then successful chase
			if (TargetIsAdjacent())
				return NodeState.Success;
			else
				return NodeState.Pending;
		}

		bool TargetIsAdjacent()
		{
			//Check if already adjacent
			var adjacentTiles = Map.GetPossibleTiles(au.currentTile.owner, au.currentTile, 1, typeof(AllyUnit));
			targets.value.Print();  //debug
			foreach (var t in targets.value)
				if (adjacentTiles.Contains(t))
					return true;       //If so then return success

			return false;
		}
		#endregion
	}
}
