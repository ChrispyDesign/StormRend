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

		[Header("Scanning")]
		[Tooltip("The max scanning increments this unit will do to find targets")]
		[SerializeField] int maxScanRange = 3;    //Means the unit can scan up to 3x moveRange

		[Header("Move")]
		[SerializeField] bool moveOn = true;

		[Header("Attack")]
		[SerializeField] bool attackOn = true;
		[Tooltip("0 = Absorb?, 1 = Melee, 1 > Ranged")]
		[SerializeField] int attackRange = 1;


		//Members
		UnitRegistry ur;
		AnimateUnit au;
		bool targetIsAdjacent = false;
		AnimateUnit target = null;


		#region Core
		public override void Initiate(BhaveAgent agent) => ur = UnitRegistry.current;

		public override void Begin()
		{
			targets.value.Clear();
			target = null;
			targetIsAdjacent = false;
		}

		public override NodeState Execute(BhaveAgent agent)
		{
			//Get this agent's unit
			au = agent.GetComponent<AnimateUnit>();		//Hacky fix to allow any enemy to use the same AI module
			if (!au) return NodeState.Failure;

			if (!Scan())
			{
				Debug.LogFormat("{0} found no opponents", au.name);
				return NodeState.Failure;   //Couldn't find any opponents
			}

			if (!Move())
				Debug.LogFormat("{0} didn't move. Obstacle in the way", au.name);
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
			//Get all opponents ignoring opponents that are in the process of dying
			var allOpponents = ur.GetAliveUnitsByType<AllyUnit>().Where(x => x.HP != 0);
			allOpponents.Print("[SCAN: All Opponents]");

			//Scan for opponents from 1x to 3x range
			for (int scan = 1; scan < maxScanRange; ++scan)
			{
				targets.value.Clear();

				//Calculate scan range
				//NOTE Can't use calculate move tiles because the settings are wrong
				//Path Blockers: Ignore crystals etc and other teammates
				var scanTiles = Map.GetPossibleTiles(au.currentTile,
						au.moveRange * scan + attackRange,
						typeof(EnemyUnit), typeof(InAnimateUnit));

				//Get opponents in range
				foreach (var o in allOpponents)
				{
					//Add if in range
					if (scanTiles.Contains(o.currentTile))
						targets.value.Add(o);
				}

				//Check if any opponent found
				if (scan == 1)
				{
					//If units within immediate range, stop scanning and start filtering
					if (targets.value.Count > 0)
						break;
				}
				else if (scan >= 2)
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
			targets.value.Print("[SCAN: In Range]");
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
					Debug.LogFormat("[SCAN: Provoke] : {0}", target.name);
					return true;        //TARGET ACQUIRED!
				}
			}

			//------------ PRIORITY 2: Adjacency
			if (TryGetAdjacentTarget(au.currentTile, out AnimateUnit outTarget))
			{
				target = outTarget;
				Debug.LogFormat("[SCAN: Adjacent] : {0}", target.name);
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
				Debug.LogFormat("[SCAN: Health] : {0}", target.name);
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
						Debug.LogFormat("[SCAN: Type] : {0}", target.name);
						return true;    //TARGET ACQUIRED!

					case ValkyrieTag v:
						target = t as AnimateUnit;
						Debug.LogFormat("[SCAN: Type] : {0}", target.name);
						return true;    //TARGET ACQUIRED!

					case SageTag s:
						target = t as AnimateUnit;
						Debug.LogFormat("[SCAN: Type] : {0}", target.name);
						return true;      //TARGET ACQUIRED!
				}
			}

			//--------- PRIORITY 5: Default, choose any target
			target = targets.value[Random.Range(0, targets.value.Count - 1)] as AnimateUnit;
			Debug.LogFormat("[SCAN: Default] : {0}", target.name);
			return true;    //TARGET FINALLY ACQUIRED!
		}

		/// <summary>
		/// Move towards the target. There should only be one target at this point
		/// </summary>
		bool Move()
		{
			if (!moveOn) return false;

			//Can't move if crippled
			if (au.isImmobilised)
			{
				Debug.LogFormat("{0} is crippled!");
				return false;
			}

			//Skip straight to attack if already adjacent
			if (targetIsAdjacent) return false;

			//Move as close as possible to the closest empty adjacent tile of the target
			au.CalculateMoveTiles();

			//Get in the agent's move range, get the tile that is the closest to the closest adjacent tile of the target while taking into account the unit types that block the path
			var closestAdjacentTileOfTarget = GetAdjacentTiles(target.currentTile, au.pathBlockingUnitTypes).
				OrderBy(targetAdjacentTile => Vector3.SqrMagnitude(targetAdjacentTile.transform.position - au.transform.position)).
				First();
			var closestTileInMoveRange = au.possibleMoveTiles.
				OrderBy(mt => Vector3.SqrMagnitude(mt.transform.position - closestAdjacentTileOfTarget.transform.position)).
				First();

			//Move if tile successfully found
			if (closestTileInMoveRange)
			{
				au.Move(closestTileInMoveRange);
				return true;
			}

			//Can't move ie. maybe blocked by crystals or other units etc.
			return false;
		}

		bool Attack()
		{
			if (!attackOn) return false;

			//Can't attack if blind
			if (au.isBlind)
			{
				Debug.LogFormat("{0} is blind!");
				return false;
			}

			//Attack immediately
			Debug.Assert(au.abilities[0], "Enemy not loaded with Ability!");

			//Attack immediately if adjacent
			if (targetIsAdjacent)
			{
				au.Act(au.abilities[0], target.currentTile);
				return true;
			}
			else
			{
				//This unit would've already moved to the best position possible so if target is within this agent's ability's ATTACK range then attack
				au.CalculateTargetTiles(au.abilities[0]);				//Attackable tiles

				if (au.possibleTargetTiles.Contains(target.currentTile))
				{
					au.FilteredAct(au.abilities[0], au.possibleTargetTiles);		//Attack!
					return true;
				}
			}
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

		Tile[] GetAdjacentTiles(Tile start, params System.Type[] pathBlockingUnitTypes)
		{
			//Get adjacent empty tiles
			return Map.GetPossibleTiles(start, 1, pathBlockingUnitTypes);
		}

		#endregion
	}
}
