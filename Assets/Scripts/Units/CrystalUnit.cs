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
		[EnumFlags, SerializeField] TargetType targetTypes = TargetType.Animates;
		private Tile[] tilesToAttack;
		#endregion

		//Core
		public void Tick()
		{
			//Be careful of this order
			turns--;

			//Trigger animations and events
			if (turns <= 0)
				animator.SetTrigger("Explode");		//This will blow up and destroy the crystal
			else
				animator.SetTrigger("Tick");		//This will attack but remain alive (probably wont' get implemented by the functionality is here anyways)
		}

		public override void TakeDamage(HealthData healthData)
		{
			base.TakeDamage(healthData);

			if (HP <= 0)
				animator.SetTrigger("Explode");
			else
				animator.SetTrigger("Tick");
		}

		/// <summary>
		/// This should be triggered by an animation event
		/// </summary>
		public void Explode()
		{
			//Determine tiles to deal effect to (regardless of unit type because it's already ignored and filtered)
			tilesToAttack = Map.GetPossibleTiles(currentTile, range, GetListOfUnitTypesToIgnore());

			DamageTargets();
			ImmobiliseTargets();
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
			bool targetAnimates = false; bool targetInAnimates = false;

			//Populate with all possible unit types
			HashSet<Type> ignoreTypes = new HashSet<Type>();
			ignoreTypes.Add(typeof(AllyUnit));
			ignoreTypes.Add(typeof(EnemyUnit));
			ignoreTypes.Add(typeof(CrystalUnit));

			//Animates
			if ((targetTypes & TargetType.Animates) == TargetType.Animates)
				targetAnimates = true;
			//InAnimates
			if ((targetTypes & TargetType.InAnimates) == TargetType.InAnimates)
				targetInAnimates = true;
			//Allies
			if (targetAnimates || (targetTypes & TargetType.Allies) == TargetType.Allies)
				ignoreTypes.Remove(typeof(AllyUnit));
			//Enemies
			if (targetAnimates || (targetTypes & TargetType.Enemies) == TargetType.Enemies)
				ignoreTypes.Remove(typeof(EnemyUnit));
			//Crystals
			if (targetInAnimates || (targetTypes & TargetType.Crystals) == TargetType.Crystals)
				ignoreTypes.Remove(typeof(CrystalUnit));

			return ignoreTypes.ToArray();
		}
	}
}