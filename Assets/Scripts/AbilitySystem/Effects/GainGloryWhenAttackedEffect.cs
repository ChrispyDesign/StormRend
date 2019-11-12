using pokoro.BhaVE.Core.Variables;
using StormRend.MapSystems.Tiles;
using StormRend.Units;
using UnityEngine;

namespace StormRend.Abilities.Effects
{
    /// <summary>
    /// Gains the specified glory amount when affected unit takes damage
    /// </summary>
    public sealed class GainGloryWhenAttackedEffect : StatusEffect
    {
        [SerializeField] int amount = 1;
        [SerializeField] BhaveInt glory = null;

        public override void OnTakeDamage(Unit affectedUnit, HealthData damageData)
        {
            Debug.Assert(glory, "No glory SOV found!");
            if (glory) glory.value += amount;
        }

        public override void Perform(Ability ability, Unit owner, Tile[] targetTiles)
        {
            //ie. If this is on the Provoke ability, it should apply to self
            AddStatusEffectToAnimateUnits(targetTiles);
        }
    }
}