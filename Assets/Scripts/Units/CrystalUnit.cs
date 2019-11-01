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
		[EnumFlags, SerializeField] TargetType invulnerableTypes = TargetType.Animates;

		//Members

		//Singleton refs
		UnitRegistry ur;
		UserInputHandler ui;

		//Core
		protected override void Awake()
		{
			base.Awake();
			ur = UnitRegistry.current;
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
			var tilesToAttack = Map.GetPossibleTiles(this.currentTile.owner, currentTile, range, GetIgnoreUnitTypes()).ToList();

			//Deal damage
			foreach (var a in ur.aliveUnits)
			{
				if (tilesToAttack.Contains(a.currentTile))
				{
					a.TakeDamage(new DamageData(this, damage));
				}
			}
		}

		Type[] GetIgnoreUnitTypes()
		{
			List<Type> targetUnits = new List<Type>();

			//Allies
			if ((invulnerableTypes & TargetType.Allies) == TargetType.Allies)
				targetUnits.Add(typeof(AllyUnit));
			//Enemies
			if ((invulnerableTypes & TargetType.Enemies) == TargetType.Enemies)
				targetUnits.Add(typeof(EnemyUnit));
			//Crystals
			if ((invulnerableTypes & TargetType.Crystals) == TargetType.Crystals)
				targetUnits.Add(typeof(CrystalUnit));
			//InAnimates
			if ((invulnerableTypes & TargetType.InAnimates) == TargetType.InAnimates)
				targetUnits.Add(typeof(InAnimateUnit));
			//Animates
			if ((invulnerableTypes & TargetType.Animates) == TargetType.Animates)
				targetUnits.Add(typeof(AnimateUnit));
				
			return targetUnits.ToArray();
		}

        public void OnPointerClick(PointerEventData eventData)
        {
			Debug.Log(this.name);
        }
    }
}