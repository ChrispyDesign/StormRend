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
		[Tooltip("HARDCODE: Number of turns before this crystal explodes")]
		public int turns = 1;
		
		[Tooltip("HARDCODE: The damage this crystal does to ")]
		public int damage = 1;

		[Tooltip("HARDCODE: The range of the damage")]
		public int range = 1;

		[Tooltip("HARDCODE: The type of units that will get damaged")]
		[EnumFlags, SerializeField] TargetType vulnerableUnitTypes = TargetType.Animates;

		//Members


		//Core
		public void Tick()
		{
			//Be careful of this order
			turns--;

			DealDamageToSurroundingUnits();

			if (turns <= 0) base.Die();
		}

		//Helpers
		void DealDamageToSurroundingUnits()
		{
			//TODO This is slightly confusing
			//Determine tiles to do damage to (regardless of unit type because it's already ignored and filtered)
			var tilesToAttack = Map.GetPossibleTiles(this.currentTile.owner, currentTile, range, GetIgnoreTypes());

			//Deal damage
			foreach (var a in ur.aliveUnits)
			{
				if (tilesToAttack.Contains(a.currentTile))
				{
					a.TakeDamage(new DamageData(this, damage));
				}
			}
		}

		Type[] GetIgnoreTypes()
		{
			//Populate with all possible unit types
			HashSet<Type> ignoreTypes = new HashSet<Type>();
			ignoreTypes.Add(typeof(AllyUnit));
			ignoreTypes.Add(typeof(EnemyUnit));
			ignoreTypes.Add(typeof(CrystalUnit));
			ignoreTypes.Add(typeof(InAnimateUnit));
			ignoreTypes.Add(typeof(AnimateUnit));

			//Allies
			if ((vulnerableUnitTypes & TargetType.Allies) == TargetType.Allies)
				ignoreTypes.Remove(typeof(AllyUnit));
			//Enemies
			if ((vulnerableUnitTypes & TargetType.Enemies) == TargetType.Enemies)
				ignoreTypes.Remove(typeof(EnemyUnit));
			//Crystals
			if ((vulnerableUnitTypes & TargetType.Crystals) == TargetType.Crystals)
				ignoreTypes.Remove(typeof(CrystalUnit));
			//InAnimates
			if ((vulnerableUnitTypes & TargetType.InAnimates) == TargetType.InAnimates)
				ignoreTypes.Remove(typeof(InAnimateUnit));
			//Animates
			if ((vulnerableUnitTypes & TargetType.Animates) == TargetType.Animates)
				ignoreTypes.Remove(typeof(AnimateUnit));
				
			return ignoreTypes.ToArray();
		}

        public void OnPointerClick(PointerEventData eventData)
        {
			Debug.Log(this.name);
        }
    }
}