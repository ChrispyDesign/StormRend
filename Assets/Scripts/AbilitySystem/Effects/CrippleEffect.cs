using StormRend.MapSystems.Tiles;
using StormRend.Units;
using UnityEngine;

namespace StormRend.Abilities.Effects
{
    public class CrippleEffect : StatusEffect
    {
		public override bool Perform(Unit owner, Tile[] targetTiles)
		{
			AddStatusAffectToAnimateUnits(targetTiles);
			return true;
		}

		public override void Perform(AnimateUnit affectedUnit)
		{
			//Cripple the bearer
			throw new System.NotImplementedException();
		}
	}
}