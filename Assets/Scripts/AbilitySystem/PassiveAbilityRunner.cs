using System.Collections.Generic;
using StormRend.Units;
using UnityEngine;

namespace StormRend.Abilities.Utilities
{
	/// <summary>
	/// This runs passive abilities on all unit creation and death
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
			passiveAbilities.Clear();
			//Cache all passive abilities
			foreach (var u in ur.aliveUnits)
			{
				//Only proceed if it is an AnimateUnit
				var au = u as AnimateUnit;
				if (au) 
					foreach (var a in au.GetAbilitiesByType(AbilityType.Passive))
						passiveAbilities.Add(a, u);
			}
		}

		public void OnUnitCreate(Unit created)
		{
			if (passiveAbilities.Count <= 0) return;
			//Go through each passive ability
			foreach (var pa in passiveAbilities)
			{
				//Auto cleanup if a unit is dead
				if (pa.Value.isDead)
				{
					// passiveAbilities.Remove(pa.Key);
					continue;
				}

				//Perform
				//NOTE: If successful will trigger animation where appropriate
				pa.Key.PerformOnUnitCreated(pa.Value, created);
			}
			//INEFFICIENT Repopulate passive ability collection
			Start();
		}

		public void OnUnitKilled(Unit killed)
		{
			if (passiveAbilities.Count <= 0) return;

			foreach (var pa in passiveAbilities)
			{
				if (pa.Value.isDead)
				{
					// passiveAbilities.Remove(pa.Key);
					continue;
				}
				pa.Key.PerformOnUnitKilled(pa.Value, killed);
			}
			Start();
		}
	}
}