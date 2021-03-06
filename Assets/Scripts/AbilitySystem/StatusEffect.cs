/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

using System.Linq;
using StormRend.MapSystems.Tiles;
using StormRend.Units;
using StormRend.Utility.Attributes;
using UnityEngine;

namespace StormRend.Abilities.Effects
{
    public abstract class StatusEffect : Effect
    {
        [ReadOnlyField, SerializeField] protected uint turnCount = 0;
        [Tooltip("Infinite if affected turn set to 0")]
        [SerializeField] protected uint affectedTurns = 1;

        #region Inflicts / Buffs / Debuffs
        /// <summary>
        /// "Inflict" status effect on victim at the beginning of the turn. 
        /// </summary>
        /// <param name="affectedUnit"></param>
        /// <returns>[false] if this status effect has expired and needs to be removed. [true] if normal operation</returns>/ 	
        public abstract bool OnStartTurn(AnimateUnit affectedUnit);

        /// "Inflict" status effect on victim right after it performed it's ability
        public virtual void OnActed(AnimateUnit affectedUnit) { }

        /// "Inflict" status effect on victim when taking damage
        public virtual bool OnTakeDamage(Unit affectedUnit, HealthData damageData)  //Unit type because crystals and blizzard can also apply damage
        {
            ++turnCount;
            if (affectedTurns > 0 && turnCount >= affectedTurns)
                return false;   //This effect has expired. Flag to be removed in AnimateUnit
            return true;
        }

        /// "Inflict" status effect on victim when taking damage
        public virtual void OnDeath(AnimateUnit affectedUnit) { }

        /// "Inflict" status effect on victim
        public abstract bool OnEndTurn(AnimateUnit affectedUnit);

        #endregion

        //Assist
        protected void AddStatusEffectToTargets(params Unit[] targetUnits)
            => AddStatusEffectToTargets(targetUnits.Select(u => u.currentTile).ToArray());

        protected void AddStatusEffectToTargets(params Tile[] targetTiles)
        {
            //Apply this status effect to ONLY animate units on the targeted tiles
            foreach (var t in targetTiles)
            {
                if (UnitRegistry.TryGetUnitTypeOnTile<AnimateUnit>(t, out AnimateUnit au))
                {
                    au.AddStatusEffect(this);
                }
            }
            //Reset turn count
            turnCount = 0;
        }
    }
}

// {
// //Increment number of turns this effect has operated
// ++turnCount;

// //Check if this status effect has expired
// if (affectedTurns > 0   //NOTE: If affectedturns set to 0 then status effect will never expire
// 	&& turnCount >= affectedTurns)
// {
// 	//Expired. Flag to be removed in AnimateUnit
// 	return false;
// }
// return true;
// }

// {
// 	//Increment number of turns this effect has operated
// 	++turnCount;

// 	//Check if this status effect has expired
// 	if (affectedTurns > 0   //NOTE: If affectedturns set to 0 then status effect will never expire
// 		&& turnCount >= affectedTurns)
// 	{
// 		//Expired. Flag to be removed in AnimateUnit
// 		return false;
// 	}
// 	return true;
// }