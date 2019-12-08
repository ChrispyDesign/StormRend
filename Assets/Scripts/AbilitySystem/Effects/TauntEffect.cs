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
	/// <summary>
	/// Deals reflex damage when affectunit is attacked
	/// </summary>
	public class TauntEffect : RuneStatusEffect
	{
		[SerializeField] int reflexDamage = 1;
		[SerializeField] string inbuiltVFXName = "FX_Provoke";

		public override void Perform(Ability ability, Unit owner, Tile[] targetTiles)
		{
			AddStatusEffectToTargets(targetTiles);
		}

		public override bool OnStartTurn(AnimateUnit affectedUnit)
		{
			//Tick this effect
			var valid = base.OnStartTurn(affectedUnit);

			if (valid)
				affectedUnit.animEventHandlers.ActivateInbuiltVFX(inbuiltVFXName);
			else
				affectedUnit.animEventHandlers.DeactivateInbuiltVFX(inbuiltVFXName);
				
			return valid;
		}

		public override bool OnTakeDamage(Unit affectedUnit, HealthData damageData)
		{
			//NOTE: THIS IS WHAT WAS CAUSING THE BLIZZARD ISSUES ON THE FINAL LEVEL!
			//The player was provoking with the berserker when the blizzard would turn over. 
			//The blizzard passes null for HealthData.vendor, hence why it freezes the game

			if (damageData.vendor == null) return false;	//Exit if it's the blizzard or other null damage dealer

			//Apply reflex damage; The victim attacks back
			damageData.vendor.TakeDamage(new HealthData(affectedUnit, reflexDamage));

			return true;
		}
	}
}