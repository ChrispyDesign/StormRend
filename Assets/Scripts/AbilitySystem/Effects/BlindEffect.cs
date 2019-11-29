using StormRend.MapSystems.Tiles;
using StormRend.Systems;
using StormRend.Units;

namespace StormRend.Abilities.Effects
{
	/// <summary>
	/// Prevents affected unit from performing abilities
	/// </summary>
	public class BlindEffect : CursedStatusEffect
	{
		public override void Perform(Ability ability, Unit owner, Tile[] targetTiles)
		{
			AddStatusEffectToTargets(targetTiles);
			BlindTargetsImmediately(targetTiles);
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