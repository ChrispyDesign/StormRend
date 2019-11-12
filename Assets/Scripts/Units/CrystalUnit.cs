using System;
using System.Collections.Generic;
using System.Linq;
using StormRend.Abilities.Effects;
using StormRend.Enums;
using StormRend.MapSystems;
using StormRend.MapSystems.Tiles;
using StormRend.Utility.Attributes;
using UnityEngine;

namespace StormRend.Units
{
	public class CrystalUnit : InAnimateUnit
	{
		//Inspector
		#region Hardcode
		[Header("Crystal")]
		[Tooltip("Number of turns till this crystal explodes")]
		[SerializeField] int turns = 1;

		[Tooltip("The amount of damage this crystal will deal when it explodes")]
		[SerializeField] int damage = 1;

		[Tooltip("Deal immobilise when it explodes")]
		[SerializeField] bool immobilise = false;

		[Tooltip("The range of this crystal's effect")]
		[SerializeField] int range = 1;

		[Tooltip("The type of units that will get damaged")]
		[EnumFlags, SerializeField] TargetType vulnerableUnitTypes = TargetType.Animates;
		private Tile[] tilesToAttack;
		#endregion

		//Core
		public void Tick()
		{
			//Be careful of this order
			turns--;

			//Determine tiles to deal effect to (regardless of unit type because it's already ignored and filtered)
			tilesToAttack = Map.GetPossibleTiles(this.currentTile.owner, currentTile, range, GetListOfUnitTypesToIgnore());

			DamageTargets();
			ImmobiliseTargets();

			//Trigger animations
			animator.SetTrigger("Explode");

			if (turns <= 0) base.Die();
		}

		//Helpers
		void ImmobiliseTargets()
		{
			if (!immobilise) return;

			foreach (var u in ur.aliveUnits)
			{
				if (tilesToAttack.Contains(u.currentTile))
				{
					var target = u as AnimateUnit;		//Cast
					var newImmobiliseEffect = ScriptableObject.CreateInstance<ImmobiliseEffect>();	//Factory create
					newImmobiliseEffect.ImmobiliseUnitImmediately(target);		//Apply effect immediately
					target.AddStatusEffect(newImmobiliseEffect);				//Add to unit's status effect collection
				}
			}
		}
		void DamageTargets()
		{
			if (damage <= 0) return;    //Slight optimisation

			foreach (var victim in ur.aliveUnits)
			{
				if (tilesToAttack.Contains(victim.currentTile))
					victim.TakeDamage(new HealthData(this, damage));
			}
		}

		Type[] GetListOfUnitTypesToIgnore()
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
	}
}