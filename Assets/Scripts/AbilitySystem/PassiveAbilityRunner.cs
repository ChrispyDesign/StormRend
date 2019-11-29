using System;
using System.Collections.Generic;
using System.Linq;
using StormRend.MapSystems;
using StormRend.MapSystems.Tiles;
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
        UnitRegistry ur;
        Dictionary<Ability, Unit> passiveAbilities = new Dictionary<Ability, Unit>();
        Dictionary<Ability, Unit> passiveAbilitiesToKeep = new Dictionary<Ability, Unit>();

        //Inits
        void Awake()
        {
            ur = GetComponent<UnitRegistry>();
        }
        void OnEnable()
        {
            ur.onUnitCreated.AddListener(OnUnitCreate);
            ur.onUnitKilled.AddListener(OnUnitKilled);

            //This would run everytime ANY unit moved... too much!
            // foreach (var au in ur.aliveUnits.Select(u => u as AnimateUnit))
            // 	au.onMoved.AddListener(OnUnitMovedRelay);
        }
        void OnDisable()
        {
            ur.onUnitCreated.RemoveListener(OnUnitCreate);
            ur.onUnitKilled.RemoveListener(OnUnitKilled);
            // foreach (var au in ur.aliveUnits.Select(u => u as AnimateUnit))
            // 	au.onMoved.RemoveListener(OnUnitMovedRelay);
        }
        void Start()
        {
            //Renew/load passive abilities
            passiveAbilities.Clear();
            //Cache all passive abilities
            foreach (var au in ur.aliveUnits.Select(x => x as AnimateUnit))
            {
                foreach (var a in au.GetAbilitiesByType(AbilityType.Passive))
                    passiveAbilities.Add(a, au);
            }

            print("Passive Abilities found: " + passiveAbilities.Count);
        }

        public void OnUnitCreate(Unit created)
        {
            if (passiveAbilities.Count <= 0) return;

            //Clear
            passiveAbilitiesToKeep.Clear();

            //Go through each passive ability
            foreach (var pa in passiveAbilities)
            {
                //Skip if unit dead
                if (pa.Value.isDead) continue;

                //Perform
                pa.Key.PerformOnUnitCreated(pa.Value, created);     //NOTE: If successful will trigger animation where appropriate

                //Keep: Unit is still alive so add to the "to keep" pile
                passiveAbilitiesToKeep.Add(pa.Key, pa.Value);
            }
            //Update passive abilities if it has changed
            if (passiveAbilities != passiveAbilitiesToKeep)     //OPTIMIZATION?
                passiveAbilities = passiveAbilitiesToKeep;
        }

        // /// <summary>
        // /// The core logic should only run for each unit
        // /// NOTE! THIS MIGHT CAUSE EXCESSIVE GARBAGE COLLECTION
        // /// </summary>
        // /// <param name="tile"></param>
        // public void OnUnitMovedRelay(Tile tile) => OnUnitMoved();
        // public void OnUnitMoved(Unit moved = null)
        // {
        // 	if (passiveAbilities.Count <= 0) return;
        // 	passiveAbilitiesToKeep.Clear();
        // 	foreach (var pa in passiveAbilities)
        // 	{
        // 		if (pa.Value.isDead) continue;
        // 		pa.Key.PerformOnUnitCreated(pa.Value, moved);
        // 		passiveAbilitiesToKeep.Add(pa.Key, pa.Value);
        // 	}
        // 	if (passiveAbilities != passiveAbilitiesToKeep)
        // 		passiveAbilities = passiveAbilitiesToKeep;
        // }

        public void OnUnitKilled(Unit killed)
        {
            print("PassiveAbilityRunner.OnUnitKilled");
            if (passiveAbilities.Count <= 0) return;
            passiveAbilitiesToKeep.Clear();
            foreach (var pa in passiveAbilities)
            {
                if (pa.Value.isDead) continue;
                pa.Key.PerformOnUnitKilled(pa.Value, killed);
                passiveAbilitiesToKeep.Add(pa.Key, pa.Value);
            }
            if (passiveAbilities != passiveAbilitiesToKeep)
                passiveAbilities = passiveAbilitiesToKeep;
        }

        //EXPERIMENTAL
        void PerformPassiveAbilitiesWithAutoCleanup(Action<Unit, Unit> passiveAction, Unit subject)
        {
            //Go through each passive ability
            var passiveAbilitiesToKeep = new Dictionary<Ability, Unit>();
            foreach (var pa in passiveAbilities)
            {
                //Skip if ability owner no longer exists in game
                if (pa.Value.isDead) continue;

                //Perform
                passiveAction(pa.Value, subject);     //NOTE: If successful will trigger animation where appropriate

                //Keep: Unit is still alive so add to the "to keep" pile
                passiveAbilitiesToKeep.Add(pa.Key, pa.Value);
            }
            //Update passive abilities 
            passiveAbilities = passiveAbilitiesToKeep;
        }
    }
}