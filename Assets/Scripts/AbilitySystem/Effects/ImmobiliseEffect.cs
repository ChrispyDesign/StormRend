using StormRend.MapSystems.Tiles;
using StormRend.Units;

namespace StormRend.Abilities.Effects
{
	/// <summary>
	/// Prevents the unit from moving
	/// </summary>
	public class ImmobiliseEffect : StatusEffect
    {
		public override void Perform(Ability ability, Unit owner, Tile[] targetTiles)
		{
			AddStatusEffectToAnimateUnits(targetTiles);
			ImmobiliseTargetUnitsImmediately(targetTiles);
		}

		public override bool OnBeginTurn(AnimateUnit affectedUnit)
		{
			var valid = base.OnBeginTurn(affectedUnit);
			if (valid)
			{
				//Cripple the bearer for this turn
				affectedUnit.SetCanMove(false);
			}
			return valid;
		}

		void ImmobiliseTargetUnitsImmediately(Tile[] targetTiles)
		{
			foreach (var tt in targetTiles)
				if (UnitRegistry.TryGetUnitTypeOnTile<AnimateUnit>(tt, out AnimateUnit au))
					au.SetCanMove(false);
		}

		public void ImmobiliseUnitImmediately(AnimateUnit au)
		{
			au.SetCanMove(false);
		}
	}
}