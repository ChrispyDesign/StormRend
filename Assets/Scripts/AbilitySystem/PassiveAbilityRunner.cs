using System.Collections.Generic;
using pokoro.BhaVE.Core.Variables;
using StormRend.Abilities.Effects;
using StormRend.Units;
using StormRend.Utility;
using StormRend.Utility.Attributes;
using UnityEngine;

namespace StormRend.Abilities.Utilities
{
	/// <summary>
	/// This runs 
	/// </summary>
	[RequireComponent(typeof(UnitRegistry))]
	public class PassiveAbilityRunner : MonoBehaviour
	{
		Dictionary<Ability, Unit> passiveAbilities = new Dictionary<Ability, Unit>();
		UnitRegistry ur;

		void Awake() => ur = GetComponent<UnitRegistry>();
		void OnEnable()
		{
			ur.onUnitCreated.AddListener(OnUnitCreate);
			ur.onUnitKilled.AddListener(OnUnitKilled);
		}
		void OnDisable()
		{
			ur.onUnitCreated.AddListener(OnUnitCreate);
			ur.onUnitKilled.RemoveListener(OnUnitKilled);
		}

		void Start()
		{
			//Cache all passive abilities
			foreach (var u in ur.aliveUnits)
			{
				var au = u as AnimateUnit;
				foreach (var a in au.GetAbilitiesByType(AbilityType.Passive))
					passiveAbilities.Add(a, u);
			}
		}

		public void OnUnitCreate(Unit created)
		{
			//Go through each passive ability
			foreach (var pa in passiveAbilities)
			{
				//Auto cleanup if unit is dead
				if (pa.Value.isDead)
				{
					passiveAbilities.Remove(pa.Key);
					continue;
				}

				//Run animation
				pa.Value.animator.SetTrigger(pa.Key.animationTrigger);

				//Perform
				pa.Key.PerformOnUnitCreated(pa.Value, created);
			}
		}

		public void OnUnitKilled(Unit killed)
		{
			//Go through each passive ability
			foreach (var pa in passiveAbilities)
			{
				//Auto cleanup if unit is dead
				if (pa.Value.isDead)
				{
					passiveAbilities.Remove(pa.Key);
					continue;
				}

				//Run animation
				pa.Value.animator.SetTrigger(pa.Key.animationTrigger);

				//Perform
				pa.Key.PerformOnUnitKilled(pa.Value, killed);
			}
		}
	}
}