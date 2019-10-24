using pokoro.BhaVE.Core;
using StormRend.MapSystems.Tiles;
using StormRend.Units;
using UnityEngine;

namespace StormRend.Abilities.Effects
{
    public class BlindEffect : StatusEffect
    {
		public override bool Perform(Unit owner, Tile[] targetTiles)
		{
			AddStatusAffectToAnimateUnits(targetTiles);
			return true;
		}

		public override void Perform(AnimateUnit victim)
		{
			//Blind the unit? Don't do anything and just skip?
			// victim.GetComponent<BhaveAgent>().
			throw new System.NotImplementedException();
		}
	}
}