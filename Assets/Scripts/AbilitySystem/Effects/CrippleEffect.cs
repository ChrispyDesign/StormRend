using StormRend.MapSystems.Tiles;
using StormRend.Units;
using UnityEngine;

namespace StormRend.Abilities.Effects
{
    public class CrippleEffect : StatusEffect
    {
		public override bool Perform(Unit owner, Tile[] targetTiles)
		{
			AddStatusEffectToAnimateUnits(targetTiles);
			return true;
		}

		public override void OnBeginTurn(AnimateUnit affectedUnit)
		{
			//Cripple the bearer
			throw new System.NotImplementedException();
		}
	}
}