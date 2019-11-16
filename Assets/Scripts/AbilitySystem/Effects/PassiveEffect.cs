using StormRend.MapSystems.Tiles;
using StormRend.Units;
using UnityEngine;

namespace StormRend.Abilities.Effects
{
	public abstract class PassiveEffect : Effect
	{
		//Could have more. This is for soul commune for now
		public virtual bool OnUnitKilled(Ability ability, Unit owner, Unit killed) { return false; }
		public virtual bool OnUnitCreated(Ability ability, Unit owner, Unit created) { return false; }

		public sealed override void Perform(Ability ability, Unit owner, Tile[] targetTiles)
		{
			Debug.LogWarning("Cannot directly perform a passive effect. Nothing done");
		}
	}
}