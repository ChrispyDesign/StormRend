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
		//AI now scans infinitely

		[Header("Move")]
		[SerializeField] bool moveOn = true;

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
                Debug.LogFormat("{0} didn't move. Obstacle in the way", this.agent.name);
			else
                Debug.LogFormat("{0} moved", this.agent.name);

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
			var allyUnits = ur.GetAliveUnitsByType<AllyUnit>().Where(x => x.HP > 0).ToList();
			var crystals = ur.GetAliveUnitsByType<CrystalUnit>().Where(x => x.HP > 0).ToList();
			targets.AddRange(allyUnits);
			targets.AddRange(crystals);

			targets.Print("[SCAN: All Opponents]");
			if (targets.Count == 0) return false;

			//----------- PRIORITY 1: Provoke
			foreach (var u in targets)
			{
				var au = u as AnimateUnit;
				if (au.isProvoking)
				{
					//Provoker found so set as the only target
					target = u;
					Debug.LogFormat("[SCAN: Provoke] : {0}", target.name);
					return true;        //TARGET ACQUIRED!
				}
			}

			//------------ PRIORITY 2: Adjacency
			if (TryGetAdjacentTarget(agent.currentTile, out Unit outTarget))
			{
				target = outTarget;
				Debug.LogFormat("[SCAN: Adjacent] : {0}", target.name);
				targetIsAdjacent = true;      //Attack in place
				return true;        //TARGET ACQUIRED!
			}

			//----------- PRIORITY 3: Health
			//Sort by health
			targets = targets.OrderBy(t => t.HP).ToList();  //Lowest to highest

			//Check there aren't multiple opponents with low health (hashsets don't allow multiples)
			HashSet<Unit> lowestHealth = new HashSet<Unit>();
			foreach (var t in targets)
				lowestHealth.Add(t);
			if (lowestHealth.Count != 1)    //If there's only one that means all the units have the same health
			{
				target = lowestHealth.ElementAt(0);
				Debug.LogFormat("[SCAN: Health] : {0}", target.name);
				return true;    //TARGET ACQUIRED!
			}
			//Multiple units of the same health detected, continue to filter by ally type

			//----------- PRIORITY 4: Ally Type
			foreach (var t in targets)
			{
				switch (t.tag)
				{
					case BerserkerTag b:
						target = t;
						Debug.LogFormat("[SCAN: Type] : {0}", target.name);
						return true;    //TARGET ACQUIRED!

					case ValkyrieTag v:
						target = t;
						Debug.LogFormat("[SCAN: Type] : {0}", target.name);
						return true;    //TARGET ACQUIRED!

					case SageTag s:
						target = t;
						Debug.LogFormat("[SCAN: Type] : {0}", target.name);
						return true;      //TARGET ACQUIRED!

					case CrystalTag c:
						target = t;
						Debug.LogFormat("[SCAN: Type] : {0}", target.name);
						return true;      //TARGET ACQUIRED!
				}
			}

			//--------- PRIORITY 5: Default, choose any target
			target = targets[Random.Range(0, targets.Count - 1)];
			Debug.LogFormat("[SCAN: Default] : {0}", target.name);
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

			//Move as close as possible to the closest empty adjacent tile of the target
			agent.CalculateMoveTiles();

			//Get in the agent's move range, get the tile that is the closest to the closest adjacent tile of the target while taking into account the unit types that block the path
			var closestAdjacentTileOfTarget = GetAdjacentTiles(target.currentTile, agent.pathBlockingUnitTypes).
				OrderBy(targetAdjacentTile => Vector3.SqrMagnitude(targetAdjacentTile.transform.position - agent.transform.position)).
				First();
			var closestTileInMoveRange = agent.possibleMoveTiles.
				OrderBy(mt => Vector3.SqrMagnitude(mt.transform.position - closestAdjacentTileOfTarget.transform.position)).
				First();

			//Move if tile successfully found
			if (closestTileInMoveRange)
			{
				agent.Move(closestTileInMoveRange);
				return true;
			}

			//Can't move ie. maybe blocked by crystals or other units etc.
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
		bool TryGetAdjacentTarget(Tile start, out Unit adjacentTarget)
		{
			var adjacentTiles = GetAdjacentTiles(start);
			foreach (var t in targets)
				if (adjacentTiles.Contains(t.currentTile))
				{
					adjacentTarget = t;
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
