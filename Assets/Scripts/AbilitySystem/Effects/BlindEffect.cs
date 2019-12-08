/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

using StormRend.MapSystems.Tiles;
using StormRend.Systems;
using StormRend.Units;
using UnityEngine;

namespace StormRend.Abilities.Effects
{
	/// <summary>
	/// Prevents affected unit from performing abilities
	/// NOTE! This needs to be placed after any damage effects
	/// </summary>
	public class BlindEffect : CursedStatusEffect
	{
		[Header("This needs to be placed AFTER any damage effects for things to work correctly")]
		[SerializeField] string note = "Read the note";
		
		public override void Perform(Ability ability, Unit owner, Tile[] targetTiles)
		{
			//Check if parent ability has piercing damage
			//If so, use the abilities updated list of targets
			var result = ParentAbilityHasPiercingDamage(ability);
			UnityEngine.Debug.Log("parentabilithaspiercingdamage: " + result);
			if (result)	targetTiles = (Tile[])ability.userObject;	//Unboxing

			AddStatusEffectToTargets(targetTiles);
			BlindTargetsImmediately(targetTiles);
		}

		bool ParentAbilityHasPiercingDamage(Ability ability)
		{
			foreach (var e in ability.effects)
			{
				var effect = e as DamageEffect;
				if (effect && effect.isPiercing)
					return true;
			}
			return false;
		}

		public override bool OnStartTurn(AnimateUnit affectedUnit)
		{
			var valid = base.OnStartTurn(affectedUnit);
			affectedUnit.SetCanAct(!valid);
			return valid;
		}

		public override bool OnEndTurn(AnimateUnit affectedUnit)
		{		
			var valid = base.OnEndTurn(affectedUnit);

			//If effect still valid then blind, else unblind
			affectedUnit.SetCanAct(!valid);
			return valid;
		}

		void BlindTargetsImmediately(params Tile[] targetTiles)
		{
			foreach (var tt in targetTiles)
				if (UnitRegistry.TryGetUnitTypeOnTile<AnimateUnit>(tt, out AnimateUnit au))
					au.SetCanAct(false);
			UserInputHandler.current.ClearSelectedUnit();
		}

		public void BlindTargetsImmediately(params AnimateUnit[] targetUnits)
		{
			foreach (var au in targetUnits)
				au.SetCanAct(false);    //Maybe this should just set au.canact it directly
			UserInputHandler.current.ClearSelectedUnit();
		}
	}
}