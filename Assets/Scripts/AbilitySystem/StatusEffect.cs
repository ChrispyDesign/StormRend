using StormRend.MapSystems.Tiles;
using StormRend.Units;
using UnityEngine;

namespace StormRend.Abilities.Effects
{
	public abstract class StatusEffect : Effect
	{
		[SerializeField] protected int affectedTurns = 1;
		protected int turnCount = 0;

	#region Inflicts / Buffs / Debuffs
		/// "Inflict" status effect on victim at the beginning of the turn
		public virtual void OnBeginTurn(AnimateUnit victim) {}

		/// "Inflict" status effect on victim right after it performed it's ability
		public virtual void OnActed(AnimateUnit victim) {}

		/// "Inflict" status effect on victim 
		public virtual void OnEndTurn(AnimateUnit victim) {}
	#endregion

		//Assist
		protected void AddStatusEffectToAnimateUnits(Tile[] targetTiles)
		{
			//Apply this status effect to any animate units on the targeted tiles
			foreach (var t in targetTiles)
			{
				if (UnitRegistry.TryGetUnitTypeOnTile<AnimateUnit>(t, out AnimateUnit au))
				{
					au.AddStatusEffect(this);
				}
			}
			//Reset turn count
			turnCount = 0;
		}
	}

}