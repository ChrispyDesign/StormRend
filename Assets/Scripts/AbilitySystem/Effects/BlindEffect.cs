using StormRend.MapSystems.Tiles;
using StormRend.Systems;
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
			BlindTargetsImmediately(targetTiles);
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

		void BlindTargetsImmediately(params Tile[] targetTiles)
		{
			foreach (var tt in targetTiles)
				if (UnitRegistry.TryGetUnitTypeOnTile<AnimateUnit>(tt, out AnimateUnit au))
					au.SetCanAct(false);
			UserInputHandler.current.ClearSelectedUnit();
		}

		public void ImmobiliseTargetsImmediately(params AnimateUnit[] targetUnits)
		{
			foreach (var au in targetUnits)
				au.SetCanAct(false);
			UserInputHandler.current.ClearSelectedUnit();
		}
	}
}