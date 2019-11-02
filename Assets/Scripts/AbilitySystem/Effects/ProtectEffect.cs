using StormRend.MapSystems.Tiles;
using StormRend.Units;

namespace StormRend.Abilities.Effects
{
	public class ProtectEffect : StatusEffect
    {
		public override void Perform(Ability ability, Unit owner, Tile[] targetTiles)
		{
			AddStatusEffectToAnimateUnits(targetTiles);
		}

		public override void OnBeginTurn(AnimateUnit affectedUnit)
		{
			base.OnBeginTurn(affectedUnit);		//Housekeeping

		}
	}
}