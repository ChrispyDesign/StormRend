/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

using pokoro.BhaVE.Core.Variables;
using StormRend.MapSystems.Tiles;
using StormRend.Units;
using UnityEngine;

namespace StormRend.Abilities.Effects
{
    /// <summary>
    /// Gains the specified glory amount when affected unit takes damage
    /// </summary>
    public sealed class GainGloryWhenAttackedEffect : RuneStatusEffect
    {
        [SerializeField] int amount = 1;
        [SerializeField] BhaveInt glory = null;

        public override void Perform(Ability ability, Unit owner, Tile[] targetTiles)
        {
            //ie. If this is on the Provoke ability, it should apply to self
            AddStatusEffectToTargets(targetTiles);
        }

        public override bool OnTakeDamage(Unit affectedUnit, HealthData damageData)
        {
            Debug.Assert(glory, "No glory SOV found!");
            if (glory) glory.value += amount;

            return true;    //Nevers expires
        }
    }
}