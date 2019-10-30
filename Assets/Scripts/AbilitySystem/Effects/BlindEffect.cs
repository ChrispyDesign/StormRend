using StormRend.MapSystems.Tiles;
using StormRend.Units;

namespace StormRend.Abilities.Effects
{
	/// <summary>
	/// Prevents affected unit from performing abilities
	/// </summary>
	public class BlindEffect : StatusEffect
    {
		public override void Perform(Unit owner, Tile[] targetTiles)
		{
			AddStatusEffectToAnimateUnits(targetTiles);
		}

		public override void OnBeginTurn(AnimateUnit affectedUnit)
		{
			base.OnBeginTurn(affectedUnit);

			//Prevent from performing abilities
			affectedUnit.SetCanAct(false);
		}
	}
}