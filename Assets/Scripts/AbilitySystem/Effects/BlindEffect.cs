using pokoro.BhaVE.Core;
using StormRend.MapSystems.Tiles;
using StormRend.Systems.StateMachines;
using StormRend.Units;
using UnityEngine;

namespace StormRend.Abilities.Effects
{
    public class BlindEffect : StatusEffect
    {
		public override void Perform(Unit owner, Tile[] targetTiles)
		{
			AddStatusEffectToAnimateUnits(targetTiles);
		}

		public override void OnBeginTurn(AnimateUnit victim)
		{
			//Blind the unit? Don't do anything and just skip?
			// victim.GetComponent<BhaveAgent>().
			throw new System.NotImplementedException();
		}
	}
}