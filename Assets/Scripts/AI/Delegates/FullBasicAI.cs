using System.Linq;
using UnityEngine;
using pokoro.BhaVE.Core.Delegates;
using pokoro.BhaVE.Core.Enums;
using pokoro.BhaVE.Core;
using StormRend.Variables;
using StormRend.Units;
using StormRend.MapSystems;
using StormRend.Utility;
using StormRend.MapSystems.Tiles;

namespace StormRend.Bhaviours
{
    /// <summary>
    /// Moves toward the closest target unit
    /// </summary>
    [CreateAssetMenu(menuName = "StormRend/AI/FullBasicAI", fileName = "FullBasicAI")]
	public sealed class FullBasicAI : BhaveAction
	{
		[SerializeField] UnitListVar targets = null;

		[Tooltip("The seeking range in terms of no. of turns")]
		[SerializeField] int rangeInTurns = 1;

		//Members
		UnitRegistry ur;
		AnimateUnit au;

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
			bool provokingFound = false;

			//Calculate scan range
			var scanTiles = au.CalculateMoveTiles(au.moveRange + 1);	//1+ because this unit can still

			//Get all opponents
			var pendingTargets = ur.GetUnitsByType<AllyUnit>().ToList();
				pendingTargets.Print();

			//Get opponents in range
			for (int i = pendingTargets.Count-1; i > 0; --i)
			{
				//If not in scan range then remove target
				if (!scanTiles.Contains(pendingTargets[i]))
					pendingTargets.Remove(pendingTargets[i]);
			}

			//Check for provoke
			foreach (var o in pendingTargets)
			{
				if (o.isProvoking)
				{
					//Provoker found so set as the only target
					var provoker = o;
					pendingTargets.Clear();
					pendingTargets.Add(o);
					break;
				}
			}

			//Check for 


			//-------------------------------------------------------------------------------------
			//[GetAllUnits] 
			//Populate targets. Check to see if opponent is within range
			Debug.Log("--------- Populating list of allies ---------");
			var allOpponentUnits = ur.GetUnitsByType<AllyUnit>();
			// targets.value.AddRange(opponentUnits);
			allOpponentUnits.Print();

			//[CheckInRange] Within range means it is standing within this agent's possible move tiles
			Debug.Log("--------- Get targets in range ------------");
			foreach (var o in allOpponentUnits)
			{
				//If within range then add to target list
				if (au.possibleMoveTiles.Contains(o.currentTile))
					targets.value.Add(o);
			}
			targets.value.Print();

			//[CheckAdjacent] Is this unit already standing next to an adjacent unit?
			Debug.Log("--------- Check adjacency -----------");
			var adjacentTiles = GetAdjacentTiles(au.currentTile);
			foreach (var tgt in adjacentTiles)
			{
				
			}

			

			//[CheckProvoke] Check if any target is provoking and filter out
			Debug.Log("--------- Checking for provoke target -----------");
			foreach (var t in targets.value)
			{
				var at = t as AnimateUnit;
				if (at.isProvoking)
				{
					//Set as the main target
					Debug.Log("[Provoker Found]");
					provokingFound = true;
					targets.value.Clear();
					targets.value.Add(t);
					
					targets.value.Print();
					break;
				}
			}

			//[SortByHealth]
			if (!provokingFound)		//Temp
			{
				Debug.Log("--------- Sort By health -----------");
				targets.value.OrderBy(t => t.HP);
				
				targets.value.Print();
			}

			//[TargetAcquired]
			Debug.Log("--------- Targets acquired -----------");

			//[Seek] Move toward best target's empty adjacent tile if possible, ready for attacking
			Debug.Log("-------- Seeking --------");
			//Keep trying to attack until successful?
			foreach (var tgt in targets.value)
			{
				//Get adjacent empty tile
				GetAdjacentTiles(null);
				
			}


			Debug.Log("-------- ");
			//Move toward
			// var closestTile =	au.possibleMoveTiles

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
			if (TargetIsAdjacent(null))
				return NodeState.Success;
			else
				return NodeState.Pending;
		}

		bool TargetIsAdjacent(Tile start)
		{
			var adjacentTiles = GetAdjacentTiles(start);
			foreach (var target in targets.value)
				if (adjacentTiles.Contains(target.currentTile))
					return true;       //A target is adjacent
			return false;	//No adjacent targets
		}

		Tile[] GetAdjacentTiles(Tile start)
		{
			//Get adjacent empty tiles
			var result = Map.GetPossibleTiles(start.owner, start, 1, typeof(Unit)).ToList();
			//Filter out any units standing on top of it
			foreach (var t in result)
			{
				// if (t.)
			}

			return result.ToArray();
		}

		#endregion
	}
}
