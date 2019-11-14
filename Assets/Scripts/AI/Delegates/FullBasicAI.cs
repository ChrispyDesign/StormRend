﻿using System.Linq;
using UnityEngine;
using pokoro.BhaVE.Core.Delegates;
using pokoro.BhaVE.Core.Enums;
using pokoro.BhaVE.Core;
using StormRend.Variables;
using StormRend.Units;
using StormRend.MapSystems;
using StormRend.Utility;
using StormRend.MapSystems.Tiles;
using System.Collections.Generic;
using StormRend.Tags;

namespace StormRend.Bhaviours
{
	/// <summary>
	/// Moves toward the closest target unit
	/// </summary>
	[CreateAssetMenu(menuName = "StormRend/AI/FullBasicAI", fileName = "FullBasicAI")]
	public sealed class FullBasicAI : BhaveAction
	{
		[SerializeField] UnitListVar targets = null;

		[Tooltip("0 = Absorb?, 1 = Melee, 1 > Ranged")]
		[SerializeField] int attackRange = 1;
		[SerializeField] int maxScanRangeMultiplier = 3;    //Means the unit can scan up to 3x moveRange

		//Members
		UnitRegistry ur;
		AnimateUnit au;
		bool targetIsAdjacent = false;
		AnimateUnit target = null;


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
			target = null;
			targetIsAdjacent = false;
		}

		public override NodeState Execute(BhaveAgent agent)
		{
			if (!Scan())
			{
				Debug.LogFormat("{0} found no opponents", au.name);
				return NodeState.Failure;   //Couldn't find any opponents
			}

			if (!MoveToward())
				Debug.LogFormat("{0} didn't move", au.name);
			else
				Debug.LogFormat("{0} moved", au.name);

			if (Attack())
			{
				Debug.LogFormat("{0} successfully attacked", au.name);
				return NodeState.Success;
			}
			else
			{
				Debug.LogFormat("{0} didn't attack", au.name);
				return NodeState.Pending;
			}
		}

		bool Scan()
		{
			//Get all opponents
			var allOpponents = ur.GetUnitsByType<AllyUnit>();
			allOpponents.Print("------ All Opponents ------");

			//Scan for opponents from 1x to 3x range
			for (int scanMultiplier = 1; scanMultiplier < maxScanRangeMultiplier; ++scanMultiplier)
			{
				targets.value.Clear();

				//Calculate scan range
				//NOTE Can't use calculate move tiles because the settings are wrong
				//Path Blockers: Ignore crystals etc and other teammates
				var scanTiles = Map.GetPossibleTiles(au.currentTile,
						au.moveRange * scanMultiplier + attackRange,
						typeof(EnemyUnit), typeof(InAnimateUnit));

				//Get opponents in range
				foreach (var o in allOpponents)
				{
					//Add if in range
					if (scanTiles.Contains(o.currentTile))
						targets.value.Add(o);
				}

				//Check if any opponent found
				if (scanMultiplier == 1)
				{
					//If units within immediate range, stop scanning and start filtering
					if (targets.value.Count > 0)
						break;
				}
				else if (scanMultiplier >= 2)
				{
					//Units within distant range
					if (targets.value.Count > 0)
					{
						//Set the closest unit as the target
						//REMEMBER order by descening means low to high ie. first element is lowest, last element is highest
						this.target = targets.value.
								OrderByDescending(t => Vector3.SqrMagnitude(t.transform.position - au.transform.position)).
								First() as AnimateUnit;     //First should be the closest
						return true;        //TARGET ACQUIRED!
					}
				}

			}
			targets.value.Print("[In Range]");
			//Exit if still nothing in range
			if (targets.value.Count == 0) return false;

			//----------- PRIORITY 1: Provoke
			foreach (var o in targets.value)
			{
				var au = o as AnimateUnit;
				if (au.isProvoking)
				{
					//Provoker found so set as the only target
					target = o as AnimateUnit;
					Debug.LogFormat("[Provoke] : {0}", target.name);
					return true;        //TARGET ACQUIRED!
				}
			}

			//------------ PRIORITY 2: Adjacency
			if (TryGetAdjacentTarget(au.currentTile, out AnimateUnit outTarget))
			{
				target = outTarget;
				Debug.LogFormat("[Adjacent] : {0}", target.name);
				targetIsAdjacent = true;      //Attack in place
				return true;        //TARGET ACQUIRED!
			}

			//----------- PRIORITY 3: Health
			//Sort by health
			targets.value = targets.value.OrderBy(t => t.HP).ToList();  //Lowest to highest

			//Check there aren't multiple opponents with low health (hashsets don't allow multiples)
			HashSet<Unit> lowestHealth = new HashSet<Unit>();
			foreach (var t in targets.value)
				lowestHealth.Add(t);
			if (lowestHealth.Count != 1)    //If there's only one that means all the units have the same health
			{
				target = lowestHealth.ElementAt(0) as AnimateUnit;
				Debug.LogFormat("[Health] : {0}", target.name);
				return true;    //TARGET ACQUIRED!
			}
			//Multiple units of the same health detected, continue to filter by ally type

			//----------- PRIORITY 4: Ally Type
			foreach (var t in targets.value)
			{
				switch (t.tag)
				{
					case BerserkerTag b:
						target = t as AnimateUnit;
						Debug.LogFormat("[Type] : {0}", target.name);
						return true;    //TARGET ACQUIRED!

					case ValkyrieTag v:
						target = t as AnimateUnit;
						Debug.LogFormat("[Type] : {0}", target.name);
						return true;    //TARGET ACQUIRED!

					case SageTag s:
						target = t as AnimateUnit;
						Debug.LogFormat("[Type] : {0}", target.name);
						return true;      //TARGET ACQUIRED!
				}
			}

			//--------- PRIORITY 5: Default, choose any target
			target = targets.value[Random.Range(0, targets.value.Count - 1)] as AnimateUnit;
			Debug.LogFormat("[Default] : {0}", target.name);
			return true;	//TARGET FINALLY ACQUIRED!
		}

		/// <summary>
		/// Move towards the target. There should only be one target at this point
		/// </summary>
		bool MoveToward()
		{
			//Can't move if crippled
			if (au.isImmobilised) return false;

			//Skip straight to attack if already adjacent
			if (targetIsAdjacent) return false;

			//Move as close as possible to the closest empty adjacent tile of the target
			au.CalculateMoveTiles();
			var closestAdjacentTileOfTarget = GetAdjacentTiles(target.currentTile).
				OrderBy(t => Vector3.SqrMagnitude(t.transform.position - au.transform.position)).
				First();
			var closestTileInRange = au.possibleMoveTiles.
				OrderBy(mt => Vector3.SqrMagnitude(mt.transform.position - closestAdjacentTileOfTarget.transform.position)).
				First();
			if (closestTileInRange)
			{
				//Move
				au.Move(closestTileInRange);
				return true;
			}

			//Can't move ie. maybe blocked by crystals etc
			return false;
		}

		bool Attack()
		{
			return false;
		}

		bool TryGetAdjacentTarget(Tile start, out AnimateUnit adjacentTarget)
		{
			var adjacentTiles = GetAdjacentTiles(start);
			foreach (var t in targets.value)
				if (adjacentTiles.Contains(t.currentTile))
				{
					adjacentTarget = t as AnimateUnit;
					return true;       //A target is adjacent
				}

			adjacentTarget = null;
			return false;   //No adjacent targets
		}

		Tile[] GetAdjacentTiles(Tile start)
		{
			//Get adjacent empty tiles
			return Map.GetPossibleTiles(start, 1);
		}

		#endregion
	}
}
