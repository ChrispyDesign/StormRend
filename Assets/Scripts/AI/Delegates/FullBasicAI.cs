using System.Linq;
using UnityEngine;
using pokoro.BhaVE.Core.Delegates;
using pokoro.BhaVE.Core.Enums;
using pokoro.BhaVE.Core;
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
		[SerializeField] bool enabled = true;

		[Header("Scan")]
		[Tooltip("How far this agent is willing to try and look for targets")]
		[SerializeField] int maxScanIterations = 5;
		[Tooltip("The range of provoke in tiles")]
		[SerializeField] int provokeRange = 6;

		[Header("Move")]
		[SerializeField] bool moveOn = true;

		[Tooltip("Extra scan range around target in case the target has obstacles around it")]
		[SerializeField] int maxExtraSeekRangeAroundTarget = 2;

		[Header("Attack")]
		[SerializeField] bool attackOn = true;
		[Tooltip("0 = Absorb?, 1 = Melee, 1 > Ranged")]
		[SerializeField] int attackRange = 1;

		//Members
		AnimateUnit unit;
		UnitRegistry ur;
		bool targetIsAdjacent = false;
		Unit target = null;
		List<Unit> targets = new List<Unit>();


		#region Core
		public override void Initiate(BhaveAgent agent)
		{
			ur = UnitRegistry.current;
		}

		public override void Begin()
		{
			targets.Clear();
			target = null;
			targetIsAdjacent = false;
		}

		public override NodeState Execute(BhaveAgent agent)
		{
			if (!enabled) return NodeState.Failure;

			//Get this agent's unit
			this.unit = agent.GetComponent<AnimateUnit>();     //Semi hacky fix to allow any enemy to use the same AI module
			if (!this.unit) return NodeState.Failure;

			if (!Scan())
			{
				Debug.LogFormat("{0} found no opponents", this.unit.name);
				return NodeState.Failure;   //Couldn't find any opponents
			}

			if (!Move())
			{
				Debug.LogFormat("{0} didn't move", this.unit.name);
			}
			else
			{
				Debug.LogFormat("{0} moved", this.unit.name);
			}

			if (Attack())
			{
				Debug.LogFormat("{0} successfully attacked", this.unit.name);
				return NodeState.Success;
			}
			else
			{
				Debug.LogFormat("{0} didn't attack", this.unit.name);
				return NodeState.Pending;
			}
		}
		#endregion

		#region AI Logic
		/*
		SCAN
		- Get ALL opponents including crystals

		- Filter OUT the ones that can't be reached; pathblocking units set to crystals and allies
			- When scan iteration = 0
				-- Provoke check
				- Check if any units are provoking in IMMEDIATE RANGE
					- select these units and stop scanning 

		- Filter IN adjacent units;
			- if one found
				- set as target
				- stop scanning
		- Sort by health
		- Filter OUT lowest health units
			- if one found with lowest health
				- set as target
				- stop scanning
		- Filter OUT by unit type
			- if one
		- FINAL CASE
			- Just pick one at random

		MOVE
		- 

		*/

		//---------------------------------------------------------------------------------
		/// <summary>	
		/// Scans for targets and runs through a process of elimination to leave with only one final target
		/// </summary>
		bool Scan()
		{
			//[PopulateTargets]
			var allyUnits = from x in ur.GetAliveUnitsByType<AllyUnit>() where x.HP > 0 select x;
			var crystals = from x in ur.GetAliveUnitsByType<CrystalUnit>() where x.HP > 0 select x;
			targets.AddRange(allyUnits);
			targets.AddRange(crystals);
			if (targets.Count <= 0)
			{
				Debug.Log("No targets found");
				return false;
			}
			targets.Print("[All Opponents]");

			//----------- PRIORITY 1: Provoke [Filter: IN, Provoke]
			var provokeScanResult = Map.GetPossibleTiles(unit.currentTile, provokeRange + attackRange);
			foreach (var u in targets)
			{
				if (provokeScanResult.Contains(u.currentTile))
				{
					var au = u as AnimateUnit;
					if (au && au.isProvoking)   //Null check and check for provoke
					{
						//Provoker found!
						target = u;
						Debug.LogFormat("[Provoke] : {0}", target.name);
						return true;        //TARGET ACQUIRED!
					}
				}
			}

			//----------- FILTER OUT UNREACHABLE TARGETS [Filter: IN, Reachable Targets]
			var scanResult = Map.GetPossibleTiles(unit.currentTile, unit.moveRange * maxScanIterations + attackRange);
			targets = (from t in targets where scanResult.Contains(t.currentTile) select t).ToList();
			targets.Print("Reachable Units");


			//------------ PRIORITY 2: Adjacency
			if (TryGetAdjacentTargets(unit.currentTile, out Unit[] adjacentTargets))
			{
				targetIsAdjacent = true;

				if (adjacentTargets.Length == 0)
				{
					//Single target found
					target = adjacentTargets[0];
					Debug.LogFormat("[Adjacent] : {0}", target.name);

					return true;        //TARGET ACQUIRED!
				}
				//Multiple adjacent targets found! Filter out and continue
				else
				{
					targets = adjacentTargets.ToList();
					Debug.LogFormat("[Adjacent] : {0}", adjacentTargets);
				}
			}

			//----------- PRIORITY 3: Health
			//Sort by health
			var animateTargets = targets.
				Where(t => t is AnimateUnit).       //Where target is an animate unit
				OrderBy(t => t.HP).ToList();        //Sort by lowest to highest health

			//Continue filtering if no animate units found
			if (animateTargets.Count > 0)
			{
				//Check there aren't multiple opponents with the same low health (hashsets don't allow multiples)
				HashSet<Unit> lowestHealth = new HashSet<Unit>();
				foreach (var t in animateTargets)
					lowestHealth.Add(t);
				if (lowestHealth.Count != 1)    //If there's only one that means all the units have the same health
				{
					target = lowestHealth.ElementAt(0);
					Debug.LogFormat("[Health] : {0}", target.name);
					return true;    //TARGET ACQUIRED!
				}
				//Multiple units of the same health detected, continue to filter by ally type
			}

			//----------- PRIORITY 4: Ally Type
			foreach (var t in targets)
			{
				switch (t.tag)
				{
					case BerserkerTag b:
						target = t;
						Debug.LogFormat("[Type] : {0}", target.name);
						return true;    //TARGET ACQUIRED!

					case ValkyrieTag v:
						target = t;
						Debug.LogFormat("[Type] : {0}", target.name);
						return true;    //TARGET ACQUIRED!

					case SageTag s:
						target = t;
						Debug.LogFormat("[Type] : {0}", target.name);
						return true;      //TARGET ACQUIRED!

					case CrystalTag c:
						target = t;
						Debug.LogFormat("[Type] : {0}", target.name);
						return true;      //TARGET ACQUIRED!
				}
			}

			targets.Print("Final");
			//--------- PRIORITY 5: Final ditch effort, This is probably a crystal, so just attack it
			if (targets.Count <= 0) return false;
			target = targets[Random.Range(0, targets.Count)];
			Debug.LogFormat("[Default] : {0}", target.name);
			return true;    //TARGET FINALLY ACQUIRED!
		}

		//----------------------------------------------------------------------------------------
		/// <summary>
		/// Move towards the target. There should only be one target at this point
		/// </summary>
		bool Move()
		{
			if (!moveOn) return false;

			//Can't move if crippled
			if (unit.isImmobilised)
			{
				Debug.LogFormat("{0} is crippled!");
				return false;
			}

			//Skip straight to attack if already adjacent
			if (targetIsAdjacent) return false;

			//Get the Closest, Empty Tile that intersects Agent's Possible Move Range & Attack Range Sized Area from Target + extra range
			//--------------- Scan Destination Tile Around Target: Keep iterating until suitable tiles that can be moved to are found around the target
			Tile[] emptyTilesAroundTarget = null;
			for (int extraSeekTiles = 0; extraSeekTiles < maxExtraSeekRangeAroundTarget; ++extraSeekTiles)
			{
				emptyTilesAroundTarget = Map.GetPossibleTiles(target.currentTile, attackRange + extraSeekTiles, typeof(Unit));
				// emptyTilesAroundTarget.Print(string.Format("EmptyTilesAroundTarget({0})", extraSeekRange));
				if (emptyTilesAroundTarget.Length > 0) break;
			}
			if (emptyTilesAroundTarget.Length == 0) return false;   //too many obstacles in the way around target

			//--------------- Around Agent
			Tile[] emptyTilesInPossibleMoveRange = null, intersectBetweenAgentAndTarget = null;
			for (int scanIterations = 1; scanIterations < maxScanIterations; ++scanIterations)
			{
				unit.CalculateMoveTiles(unit.moveRange * scanIterations);
				// agent.possibleMoveTiles.Print(string.Format("{0}.possibleMoveTiles", agent.name));
				if (unit.possibleMoveTiles.Count <= 0) return false;       //agent is locked AND/OR can only attack

				//Filter out the empty tiles; Some tiles could potentially have fellow enemies standing on it as well
				emptyTilesInPossibleMoveRange = unit.possibleMoveTiles.
					Where(t => !UnitRegistry.IsAnyUnitOnTile(t)).ToArray();
				// emptyTilesInPossibleMoveRange.Print(string.Format("emptyTilesInPossibleMoveRange({0})", scanIterations));
				if (emptyTilesInPossibleMoveRange.Length <= 0) return false;    //agent is locked possibly by other teammates

				//Keep trying to find a path to the target
				intersectBetweenAgentAndTarget = emptyTilesInPossibleMoveRange.
					Intersect(emptyTilesAroundTarget).ToArray();
				// intersectBetweenAgentAndTarget.Print("Intersect");
				if (intersectBetweenAgentAndTarget.Length > 0) break;
			}
			if (intersectBetweenAgentAndTarget.Length <= 0) return false;   //Target too far away

			//The tile to seek towards
			var seekTile = intersectBetweenAgentAndTarget.
				OrderBy(t => Vector3.SqrMagnitude(t.transform.position - unit.transform.position)).
				First();

			//Only move within range
			var finalMoveToTile = unit.CalculateMoveTiles().
				OrderBy(t => Vector3.SqrMagnitude(t.transform.position - seekTile.transform.position)).
				First();

			//Move if suitable tile successfully found
			if (finalMoveToTile)
			{
				unit.Move(finalMoveToTile);
				return true;
			}

			//Possibly too many obstacles in general (would've already checked for this)
			return false;
		}

		//---------------------------------------------------------------------------------------
		/// <summary>
		/// Attacks the target if possible
		/// </summary>
		bool Attack()
		{
			if (!attackOn) return false;

			//Can't attack if blind
			if (unit.isBlind)
			{
				Debug.LogFormat("{0} is blind!");
				return false;
			}

			Debug.Assert(unit.abilities[0], "Enemy not loaded with Ability!");

			//Attack immediately if adjacent
			if (targetIsAdjacent)
			{
				//ATTACK
				unit.Act(unit.abilities[0], target.currentTile);

				return true;
			}
			else
			{
				//This unit would've already moved to the best position possible so if target is within this agent's ability's ATTACK range then attack
				unit.CalculateTargetTiles(unit.abilities[0]);               //Attackable tiles

				if (unit.possibleTargetTiles.Contains(target.currentTile))
				{
					//ATTACK
					unit.FilteredAct(unit.abilities[0], target.currentTile);        //Attack!

					return true;
				}
			}
			return false;
		}
		#endregion

		#region Assists
		bool TryGetAdjacentTargets(Tile subject, out Unit[] adjacentTargets)
		{
			var adjacentTiles = Map.GetPossibleTiles(subject, 1);
			var interimAdjacentTargets = new List<Unit>();

			foreach (var t in this.targets)
			{
				if (adjacentTiles.Contains(t.currentTile))
					interimAdjacentTargets.Add(t);
			}

			//Adjacent targets found
			if (interimAdjacentTargets.Count > 0)
			{
				adjacentTargets = interimAdjacentTargets.ToArray();
				return true;
			}
			//No adjacent targets found
			adjacentTargets = null;
			return false;   //No adjacent targets
		}
		#endregion
	}
}
