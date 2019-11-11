using StormRend.MapSystems.Tiles;
using StormRend.Units;
using UnityEngine;

namespace StormRend.Abilities.Effects
{
	public abstract class PassiveEffect : Effect
	{
		//Could have more. This is for soul commune for now
		public virtual void OnUnitKilled(Ability ability, Unit owner, Unit killed) {}
		public virtual void OnUnitCreated(Ability ability, Unit owner, Unit created) {}

		public sealed override void Perform(Ability ability, Unit owner, Tile[] targetTiles)
		{
			Debug.LogWarning("Cannot directly perform a passive effect. Nothing done");
		}
	}
}