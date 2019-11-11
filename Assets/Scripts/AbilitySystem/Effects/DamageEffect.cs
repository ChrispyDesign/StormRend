using System;
using System.Collections.Generic;
using System.Linq;
using pokoro.BhaVE.Core.Variables;
using StormRend.MapSystems.Tiles;
using StormRend.Units;
using StormRend.Utility;
using StormRend.Utility.Attributes;
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

		// public override void Prepare(Ability ability, Unit owner)
		// {
		// 	Debug.Assert(glory, "No glory SOV allocated!");
		// }

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

			if (piercing)
			{
				//Only get the first target
				var target = targetTiles[0];

				//Round the coords it to the nearest 1 or zero
				Vector3 dirVector = (target.transform.position - owner.currentTile.transform.position).normalized;  //Get normalized direction
				Vector2Int attackDirection = new Vector2(dirVector.x, dirVector.z).ToVector2Int();  //Convert to v2int direction

				//Attack all units in that direction
				var workingTile = owner.currentTile;        //Start at owner's tile
				while (workingTile.TryGetTile(attackDirection, out Tile t, true))   //Keep getting tile in direction of the attack
				{
					//Check is in the list of possible targets
					if (au.possibleTargetTiles.Contains(t))
					{
						//If a unit is on top then attack
						foreach (var u in units)
						{
							if (t == u.currentTile)
							{
								u.TakeDamage(new DamageData(owner, damage));

								HandleGainGlory(u);
							}
						}
					}
					workingTile = t;    //Try getting from the new tile
				}
			}
			else
			{
				List<Tile> tt = targetTiles.ToList();
				foreach (var u in units)
				{
					if (tt.Contains(u.currentTile))
					{
						//Damage units that are standing on target tiles
						u.TakeDamage(new DamageData(owner, damage));

						HandleGainGlory(u);
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
	}
}

