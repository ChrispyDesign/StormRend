using System.Collections.Generic;
using pokoro.BhaVE.Core.Variables;
using StormRend.Abilities.Effects;
using StormRend.Units;
using StormRend.Utility;
using UnityEngine;

namespace StormRend.Abilities.Utilities
{
	[RequireComponent(typeof(UnitRegistry))]
    public class PassiveEffectRunner : MonoBehaviour
    {
		Dictionary<Ability, Unit> passiveAbilities = new Dictionary<Ability, Unit>();
        UnitRegistry ur;

		void Awake() => ur = GetComponent<UnitRegistry>();
		// void OnEnable()
		// {
		// 	ur.onUnitCreated.AddListener(OnUnitCreate);
		// 	ur.onUnitKilled.AddListener(OnUnitKilled);
		// }
		// void OnDisable()
		// {
		// 	ur.onUnitCreated.AddListener(OnUnitCreate);
		// 	ur.onUnitKilled.RemoveListener(OnUnitKilled);
		// }

		void Start()
		{
			//Cache all passive abilities
			foreach (var u in ur.aliveUnits)
			{
				var au = u as AnimateUnit;

				foreach (var a in au.GetAbilitiesByType(AbilityType.Passive))
				{
					passiveAbilities.Add(a, u);
				}
			}
		}

		public void OnUnitCreate(Unit created)
		{
			//Go through each passive ability
			foreach (var pa in passiveAbilities)
			{
				//Perform on unit killed for each ability
				pa.Key.PerformOnUnitKilled(pa.Value, created);
			}
		}

		public void OnUnitKilled(Unit killed)
		{
			//Go through each passive ability
			foreach (var pa in passiveAbilities)
			{
				//Perform on unit killed for each ability
				pa.Key.PerformOnUnitKilled(pa.Value, killed);
			}
		}
    }
}