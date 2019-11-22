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
		[Header("Scan")]
		[Tooltip("Essentially the how far this agent is willing to try and look for targets")]
		[SerializeField] int maxScanIterations = 5;

		[Header("Move")]
		[SerializeField] bool moveOn = true;

		[Tooltip("Extra scan range around target in case the target has obstacles around it")]
		[SerializeField] int maxExtraSeekRangeAroundTarget = 2;

		[Header("Attack")]
		[SerializeField] bool attackOn = true;
		[Tooltip("0 = Absorb?, 1 = Melee, 1 > Ranged")]
		[SerializeField] int attackRange = 1;

		//Members
		AnimateUnit agent;
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
			//Get this agent's unit
			this.agent = agent.GetComponent<AnimateUnit>();     //Hacky fix to allow any enemy to use the same AI module
			if (!this.agent) return NodeState.Failure;

			if (!Scan())
			{
				Debug.LogFormat("{0} found no opponents", this.agent.name);
				return NodeState.Failure;   //Couldn't find any opponents
			}

			if (!Move())
			{
				Debug.LogFormat("{0} didn't move", this.agent.name);
			}
			else
			{
				Debug.LogFormat("{0} moved", this.agent.name);
			}

			if (Attack())
			{
				Debug.LogFormat("{0} successfully attacked", this.agent.name);
				return NodeState.Success;
			}
			else
			{
				Debug.LogFormat("{0} didn't attack", this.agent.name);
				return NodeState.Pending;
			}
		}
		#endregion

		#region AI Logic
		//---------------------------------------------------------------------------------
		/// <summary>	
		/// Scans for targets and runs through a process of elimination to leave with only one final target
		/// </summary>
		bool Scan()
		{
			//Get all units
			var allyUnits = ur.GetAliveUnitsByType<AllyUnit>().Where(x => x.HP > 0).ToList();
			var crystals = ur.GetAliveUnitsByType<CrystalUnit>().Where(x => x.HP > 0).ToList();
			targets.AddRange(allyUnits);
			targets.AddRange(crystals);
			targets.Print("[All Opponents]");
			if (targets.Count == 0) return false;

			//FILTER REACHABLE TARGETS; Make sure the targets can actually be reached
			// var maxReachTiles = Map.GetPossibleTiles(agent.currentTile, agent.moveRange * maxScanIterations);
			// // agent.CalculateMoveTiles((agent.moveRange * maxScanIterations));
			// maxReachTiles.Print("Max reachable tiles");
			// // List<Unit> reachableTargets = new List<Unit>();
			// // foreach (var t in targets)
			// // {
			// // 	if (maxReachTiles.Contains(t.currentTile))
			// // 	reachableTargets.Add(t);
			// // }
			// // targets = reachableTargets;
			// targets = targets.Where(t => maxReachTiles.Contains(t.currentTile)).ToList();
			// targets.Print("Reachable Units");

			//----------- PRIORITY 1: Provoke
			foreach (var u in targets)
			{
				var au = u as AnimateUnit;
				if (au && au.isProvoking)	//Null check and check for provoke
				{
					//Provoker found so set as the only target
					target = u;
					Debug.LogFormat("[Provoke] : {0}", target.name);
					return true;        //TARGET ACQUIRED!
				}
			}

			//------------ PRIORITY 2: Adjacency
			if (TryGetAdjacentTargets(agent.currentTile, out Unit[] outTargets))
			{
				if (outTargets.Length == 0)
				{
					//Single target found
					target = outTargets[0];
					Debug.LogFormat("[Adjacent] : {0}", target.name);
					targetIsAdjacent = true;      //Attack in place
					return true;        //TARGET ACQUIRED!
				}
				//Multiple adjacent targets found! Continue filtering...
			}

			//----------- PRIORITY 3: Health
			//Sort by health
			var animateTargets = targets.
				Where(t => t is AnimateUnit).		//Where target is an animate unit
				OrderBy(t => t.HP).ToList();  		//Sort by lowest to highest health
			
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
			target = targets[Random.Range(0, targets.Count-1)];
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
			if (agent.isImmobilised)
			{
				Debug.LogFormat("{0} is crippled!");
				return false;
			}

			//Skip straight to attack if already adjacent
			if (targetIsAdjacent) return false;

			//Get the Closest, Empty Tile that intersects Agent's Possible Move Range & Attack Range Sized Area from Target + extra range
			//--------------- Around Target
			//Keep iterating until suitable tiles that can be moved to are found around the target
			Tile[] emptyTilesAroundTarget = null;
			for (int extraSeekRange = 0; extraSeekRange < maxExtraSeekRangeAroundTarget; ++extraSeekRange)
			{
				emptyTilesAroundTarget = Map.GetPossibleTiles(target.currentTile, attackRange + extraSeekRange, typeof(Unit));
			// emptyTilesAroundTarget.Print(string.Format("EmptyTilesAroundTarget({0})", extraSeekRange));
				if (emptyTilesAroundTarget.Length > 0) break;
			}
			//Exit if too many obstacles in the way around target
			if (emptyTilesAroundTarget.Length == 0) return false;

			//--------------- Around Agent
			Tile[] emptyTilesInPossibleMoveRange = null, intersectBetweenAgentAndTarget = null;
			for (int scanIterations = 1; scanIterations < maxScanIterations; ++scanIterations)
			{
				agent.CalculateMoveTiles(agent.moveRange * scanIterations);
			// agent.possibleMoveTiles.Print(string.Format("{0}.possibleMoveTiles", agent.name));
				if (agent.possibleMoveTiles.Length <= 0) return false;		//This means the unit is locked AND/OR can only attack

				//Filter out the empty tiles; Some tiles could potentially have fellow enemies standing on it as well
				emptyTilesInPossibleMoveRange = agent.possibleMoveTiles.Where(t => !UnitRegistry.IsAnyUnitOnTile(t)).ToArray();
			// emptyTilesInPossibleMoveRange.Print(string.Format("emptyTilesInPossibleMoveRange({0})", scanIterations));
				if (emptyTilesInPossibleMoveRange.Length <= 0) return false;	//agent is locked possibly by other teammates

				//Keep trying to find a path to the target
				intersectBetweenAgentAndTarget = emptyTilesInPossibleMoveRange.Intersect(emptyTilesAroundTarget).ToArray();
			// intersectBetweenAgentAndTarget.Print("Intersect");
				if (intersectBetweenAgentAndTarget.Length > 0) break;
			}
			if (intersectBetweenAgentAndTarget.Length <= 0) return false;	//Target too far away

			//The tile to seek towards
			var seekTile = intersectBetweenAgentAndTarget.
				OrderBy(t => Vector3.SqrMagnitude(t.transform.position - agent.transform.position)).
				First();

			//Only move within range
			var finalMoveToTile = agent.CalculateMoveTiles().
				OrderBy(t => Vector3.SqrMagnitude(t.transform.position - seekTile.transform.position)).
				First();

			//Move if tile suitable successfully found
			if (finalMoveToTile)
			{
				agent.Move(finalMoveToTile);
				return true;
			}

			//Possibly too many obstacles in general
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
			if (agent.isBlind)
			{
				Debug.LogFormat("{0} is blind!");
				return false;
			}

			Debug.Assert(agent.abilities[0], "Enemy not loaded with Ability!");

			//Attack immediately if adjacent
			if (targetIsAdjacent)
			{
				//ATTACK
				agent.Act(agent.abilities[0], target.currentTile);

				return true;
			}
			else
			{
				//This unit would've already moved to the best position possible so if target is within this agent's ability's ATTACK range then attack
				agent.CalculateTargetTiles(agent.abilities[0]);               //Attackable tiles

				if (agent.possibleTargetTiles.Contains(target.currentTile))
				{
					//ATTACK
					agent.FilteredAct(agent.abilities[0], target.currentTile);        //Attack!

					return true;
				}
			}
			return false;
		}
		#endregion

		#region Assists
		bool TryGetAdjacentTargets(Tile start, out Unit[] adjacentTarget)
		{
			var adjacentTiles = GetAdjacentTiles(start);
			var interimAdjacentTargets = new List<Unit>();

			foreach (var t in targets)
			{
				if (adjacentTiles.Contains(t.currentTile))
					interimAdjacentTargets.Add(t);
			}
			
			//Adjacent targets found
			if (interimAdjacentTargets.Count > 0)
			{
				adjacentTarget = interimAdjacentTargets.ToArray();
				return true;
			}
			//No adjacent targets found
			adjacentTarget = null;
			return false;   //No adjacent targets
		}

		Tile[] GetAdjacentTiles(Tile start, params System.Type[] pathBlockingUnitTypes)
		{
			//Get adjacent empty tiles
			return Map.GetPossibleTiles(start, 1, pathBlockingUnitTypes);
		}
		#endregion
	}
}
