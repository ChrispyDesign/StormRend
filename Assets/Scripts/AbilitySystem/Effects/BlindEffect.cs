using StormRend.MapSystems.Tiles;
using StormRend.Units;

namespace StormRend.Abilities.Effects
{
	/// <summary>
	/// Prevents affected unit from performing abilities
	/// </summary>
	public class BlindEffect : StatusEffect
	{
		public override void Perform(Ability ability, Unit owner, Tile[] targetTiles)
		{
			AddStatusEffectToAnimateUnits(targetTiles);
			BlindTargetUnitsImmediately(targetTiles);
		}

		public override bool OnBeginTurn(AnimateUnit affectedUnit)
		{
			var valid = base.OnBeginTurn(affectedUnit);
			if (valid)
			{
				//Prevent from performing abilities
				affectedUnit.SetCanAct(false);
			}
			return valid;
		}

		void BlindTargetUnitsImmediately(Tile[] targetTiles)
		{
			foreach (var tt in targetTiles)
				if (UnitRegistry.TryGetUnitTypeOnTile<AnimateUnit>(tt, out AnimateUnit au))
					au.SetCanAct(false);
		}
	}
}