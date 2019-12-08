/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

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
            //This needs to trigger so that the health bars update properly
            affectedUnit.onTakeDamage.Invoke(damageData); 

            //Play some kind of protect effect/animation?
            affectedUnit.animator.ResetTrigger("HitReact");     //Prevent HitReact animation from playing
            // affectedUnit.animator.SetTrigger("Parry");		//TODO Maybe play some kind of block?

            return base.OnTakeDamage(affectedUnit, damageData);
        }
    }
}