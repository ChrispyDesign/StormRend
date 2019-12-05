using System;
using System.Collections.Generic;
using System.Linq;
using pokoro.BhaVE.Core.Variables;
using StormRend.MapSystems.Tiles;
using StormRend.Units;
using StormRend.Utility;
using UnityEngine;

namespace StormRend.Abilities.Effects
{
    public class DamageEffect : Effect
	{
		//Enums
		[Flags]
		public enum GainGloryType
		{
			None = 1 << 0,
			Hit = 1 << 1,
			Kill = 1 << 2,
			HitAndKill = 1 << 3,
		}

		[SerializeField] int damage = 1;

		[Tooltip("Deals multiple damage inline. DO NOT use on diagonals! Will freeze Unity (infinity loop)")]
		[SerializeField] bool piercing = false;

		[Space(5)]
		[SerializeField] GainGloryType gainGlory = GainGloryType.None;
		[SerializeField] int gloryAmount = 1;
		[SerializeField] BhaveInt glory = null;

		//To send to blinding effect
		public bool isPiercing => piercing;

		public override void Perform(Ability ability, Unit owner, Tile[] targetTiles)
		{
			var au = owner as AnimateUnit;

			if (targetTiles.Length == 0)
			{
				Debug.LogWarning("Need at least one target tile");
				return;
			}

			//Get and convert to lists where required
			Unit[] units = UnitRegistry.current.aliveUnits;

			//TODO This can occasionally get stuck in a infinite loop
			//PIERCING ATTACK
			if (piercing)
			{
				//Only get the first target
				var target = targetTiles[0];

				//Round the coords it to the nearest 1 or zero. Make sure to flatten so that they're all on the same playing field
				var flatTargetTilePos = new Vector3(target.transform.position.x, 0, target.transform.position.z);
				var flatOwnerTilePos = new Vector3(owner.currentTile.transform.position.x, 0, owner.currentTile.transform.position.z);

				Vector3 dirVector = (flatTargetTilePos - flatOwnerTilePos).normalized;
				Vector2Int attackDirection = new Vector2Int(Mathf.RoundToInt(dirVector.x), Mathf.RoundToInt(dirVector.z));

				// Debug.Log("Attack Direction: " + attackDirection);

				//Detect 0,0 error
				if (attackDirection.x == 0 && attackDirection.y == 0)
				{
					Debug.LogWarning("Invalid attack direction (0, 0)! Exiting...");
					Debug.Log("dirVector: " + dirVector);
					Debug.Log("target.transform.position: " + target.transform.position);
					Debug.Log("owner.currentTile.transform.position: " + owner.currentTile.transform.position);
					return;
				}

				//Attack all units in that direction
				var newLastTargets = new List<Tile>();
				var workingTile = owner.currentTile;        //Start at owner's tile
				int i = 0;
				while (workingTile.TryGetTile(attackDirection, out Tile t, true))   //Keep getting tile in direction of the attack
				{
					//Check is in the list of possible targets
					if (au.possibleTargetTiles.Contains(t))
					{
						//Valid tile, add to list of last targets
						newLastTargets.Add(t);

						//If a unit is on top then attack
						foreach (var victim in units)
						{
							if (t == victim.currentTile)
							{
								victim.TakeDamage(new HealthData(owner, damage));

								HandleGainGlory(victim);

                                //If victim was killed then invoke owner's on kill event
								//HARDCODE: ENEMY FILTER
                                if (victim.isDead && victim is EnemyUnit) SetJustKilled(owner, victim);
							}
						}
					}
					workingTile = t;    //Try getting from the new tile

					//Infinite loop debug
					if (i > 3)
					{
						Debug.LogWarning("Infinite loop detected!");
						Debug.LogWarningFormat("Target Tile: {0}", targetTiles[0]);
						Debug.LogWarningFormat("Direction Vector: {0}", dirVector);
						Debug.LogWarningFormat("Attack Direction: {0}", attackDirection);
						break;
					}
					else
						++i;
				}

				//Update last targets of parent ability
				ability.userObject = newLastTargets.ToArray();
			}
			//NORMAL ATTACK
			else
			{
				List<Tile> tt = targetTiles.ToList();
				foreach (var victim in units)
				{
					if (tt.Contains(victim.currentTile))
					{
						//Damage units that are standing on target tiles
						victim.TakeDamage(new HealthData(owner, damage));

						HandleGainGlory(victim);

						//If victim was killed then invoke owner's on kill event
                        if (victim.isDead & victim is EnemyUnit) SetJustKilled(owner, victim);
					}
				}
			}
		}

		void HandleGainGlory(Unit u)
		{
			//HIT
			if ((gainGlory & GainGloryType.Hit) == GainGloryType.Hit ||
				(gainGlory & GainGloryType.HitAndKill) == GainGloryType.HitAndKill)
			{
				if (glory) glory.value += gloryAmount;
			}
			//KILL
			if ((gainGlory & GainGloryType.Kill) == GainGloryType.Kill ||
				(gainGlory & GainGloryType.HitAndKill) == GainGloryType.HitAndKill
				&& u.isDead)
			{
				if (glory) glory.value += gloryAmount;
			}
		}

		void SetJustKilled(Unit owner, Unit victim)
		{
			owner.hasJustKilled = true;
			owner.onEnemyKilled.Invoke(victim);
		}
	}
}
