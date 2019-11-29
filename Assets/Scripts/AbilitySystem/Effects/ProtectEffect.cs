using StormRend.MapSystems.Tiles;
using StormRend.Units;
using UnityEngine;

namespace StormRend.Abilities.Effects
{
    public class ProtectEffect : RuneStatusEffect
    {
        [SerializeField] bool applyToSelf = false;

        public override void Perform(Ability ability, Unit owner, Tile[] targetTiles)
        {
            if (applyToSelf)
                AddStatusEffectToTargets(owner);
            else
                AddStatusEffectToTargets(targetTiles);		//This also should apply the effect immediately
        }

        public override bool OnStartTurn(AnimateUnit affectedUnit)
        {
            //Tick this effect
            return base.OnStartTurn(affectedUnit);
        }

        public override bool OnTakeDamage(Unit affectedUnit, HealthData damageData)
        {
            //Reverse the damage done
            affectedUnit.HP += damageData.amount;

            //Play some kind of protect effect/animation?
            affectedUnit.animator.ResetTrigger("HitReact");     //Prevent HitReact animation from playing
            // affectedUnit.animator.SetTrigger("Parry");		//TODO Maybe play some kind of block?

            return base.OnTakeDamage(affectedUnit, damageData);
        }
    }
}