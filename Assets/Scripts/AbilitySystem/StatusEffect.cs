using StormRend.MapSystems.Tiles;
using StormRend.Units;
using UnityEngine;

namespace StormRend.Abilities.Effects
{
	public abstract class StatusEffect : Effect
	{
		[SerializeField] protected int affectedTurns = 1;
		protected int turnCount = 0;

		/// <summary>
		/// "Inflict" status effect on victim
		/// </summary>
		public abstract void Perform(AnimateUnit victim);

		protected void AddStatusAffectToAnimateUnits(Tile[] targetTiles)
		{
			//Apply this status effect to any animate units on the targeted tiles
			foreach (var t in targetTiles)
			{
				if (UnitRegistry.IsUnitTypeOnTile<AnimateUnit>(t, out AnimateUnit au))
				{
					au.AddStatusEffect(this);
				}
			}

			//Reset turn count
			turnCount = 0;
		}
	}

}