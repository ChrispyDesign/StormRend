﻿using System.Linq;
using StormRend.MapSystems.Tiles;
using StormRend.Systems;
using StormRend.Units;

namespace StormRend.Abilities.Effects
{
	/// <summary>
	/// Prevents the unit from moving
	/// </summary>
	public class ImmobiliseEffect : CursedStatusEffect
    {
		public override void Perform(Ability ability, Unit owner, Tile[] targetTiles)
		{
			AddStatusEffectToTargets(targetTiles);
			ImmobiliseTargetsImmediately(targetTiles);		//Just in case
		}

		public override bool OnStartTurn(AnimateUnit affectedUnit)
		{
			var valid = base.OnStartTurn(affectedUnit);
			affectedUnit.SetCanMove(!valid);
			return valid;
		}

		public override bool OnEndTurn(AnimateUnit affectedUnit)
		{
			var valid = base.OnEndTurn(affectedUnit);
			affectedUnit.SetCanMove(!valid);
			return valid;
		}

		public void ImmobiliseTargetsImmediately(params Tile[] targetTiles)
		{
			foreach (var tt in targetTiles)
				if (UnitRegistry.TryGetUnitTypeOnTile<AnimateUnit>(tt, out AnimateUnit au))
					au.SetCanMove(false);
			UserInputHandler.current.ClearSelectedUnit();
		}

		public void ImmobiliseTargetsImmediately(params AnimateUnit[] targetUnits)
		{
			foreach (var au in targetUnits)
				au.SetCanMove(false);
			UserInputHandler.current.ClearSelectedUnit();
		}
	}
}