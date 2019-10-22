using System;
using System.Collections.Generic;
using System.Linq;
using StormRend.Enums;
using StormRend.MapSystems;
using StormRend.Systems;
using StormRend.Utility.Attributes;
using UnityEngine;
using UnityEngine.EventSystems;

namespace StormRend.Units
{
	public class CrystalUnit : InAnimateUnit, IPointerClickHandler	//Unit =~= InAnimateUnit?
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

		//Singleton refs
		UnitRegistry ur;
		UserInputHandler ui;

		//Core
		void Awake()
		{
			ur = UnitRegistry.current;
			// ui = UserInputHandler.current;
		}

		public void Tick()
		{
			//Be careful of this order
			turns--;

			DealDamageToSurroundingUnits();

			if (turns <= 0) Die();
		}

		//Helpers
		void DealDamageToSurroundingUnits()
		{
			//TODO This is slightly confusing
			//Determine tiles to do damage to (regardless of unit type because it's already ignored and filtered)
			var tilesToAttack = Map.GetPossibleTiles(this.currentTile.owner, currentTile, 1, GetIgnoreUnitTypes()).ToList();

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

        public void OnPointerClick(PointerEventData eventData)
        {
			Debug.Log(this.name);
        }
    }
}