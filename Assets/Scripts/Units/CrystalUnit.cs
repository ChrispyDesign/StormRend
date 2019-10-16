using System;
using System.Collections.Generic;
using StormRend.Enums;
using StormRend.Systems.Mapping;
using StormRend.Utility.Attributes;
using UnityEngine;

namespace StormRend.Units
{
	public class CrystalUnit : Unit	//Unit =~= InAnimateUnit?
	{
		//Inspector
		[Header("Crystal")]
		[Tooltip("Number of turns before this crystal explodes")]
		public int turns = 1;
		[Tooltip("The damage this crystal does to ")]
		public int damage = 1;
		[Tooltip("The range of the damage")]
		public int range = 1;
		[Tooltip("The type of units that won't get damaged")]
		[EnumFlags] public TargetUnitMask ignoreUnitMask;

		//Members
		UnitRegistry ur;

		//Core
		void Awake()
		{
			ur = UnitRegistry.singleton;
		}

		public void Tick()
		{
			//Be careful of this order
			turns--;

			DealDamage();

			if (turns <= 0) Die();
		}

		//Helpers
		void DealDamage()
		{
			//TODO This is slightly confusing
			//Determine tiles to do damage to (regardless of unit type because it's already ignored and filterd)
			var tilesToAttack = Map.GetValidMoves(this.currentTile.owner, currentTile, 1, GetIgnoreUnitTypes());

			//Deal damage
			foreach (var a in ur.aliveUnits)
			{
				if (tilesToAttack.Contains(a.currentTile))
				{
					a.TakeDamage(damage);
				}
			}
		}

		Type[] GetIgnoreUnitTypes()
		{
			List<Type> targetUnits = new List<Type>();

			//Add allies
			if ((ignoreUnitMask & TargetUnitMask.Allies) == TargetUnitMask.Allies)
				targetUnits.Add(typeof(AllyUnit));
			//Add enemies
			if ((ignoreUnitMask & TargetUnitMask.Enemies) == TargetUnitMask.Enemies)
				targetUnits.Add(typeof(EnemyUnit));
			//Add crystals
			if ((ignoreUnitMask & TargetUnitMask.Allies) == TargetUnitMask.Allies)
				targetUnits.Add(typeof(CrystalUnit));
			return targetUnits.ToArray();
		}
	}
}