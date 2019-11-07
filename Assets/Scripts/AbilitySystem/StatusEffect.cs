using StormRend.MapSystems.Tiles;
using StormRend.Units;
using StormRend.Utility.Attributes;
using UnityEngine;

namespace StormRend.Abilities.Effects
{
	public abstract class StatusEffect : Effect
	{
		[ReadOnlyField, SerializeField] protected int turnCount = 0;
		[SerializeField] protected int affectedTurns = 1;

	#region Inflicts / Buffs / Debuffs
		/// "Inflict" status effect on victim at the beginning of the turn
		public virtual void OnBeginTurn(AnimateUnit affectedUnit) 
		{
			Debug.Log("StatusEffect.BeginTurn: " + this.name);

			//On start check if this status effect has expired
			if (turnCount >= affectedTurns)
			{
				//Expired. Remove from unit
				affectedUnit.statusEffects.Remove(this);
				return;
			}
			else
			{
				//Increment number of turns this effect has operated
				++turnCount;
			}
		}

		/// "Inflict" status effect on victim right after it performed it's ability
		public virtual void OnActed(AnimateUnit affectedUnit) {}

		/// "Inflict" status effect on victim when taking damage
		public virtual void OnTakeDamage(Unit affectedUnit, DamageData damageData) {}	//Unit type because crystals and blizzard can also apply damage

		/// "Inflict" status effect on victim when taking damage
		public virtual void OnDeath(AnimateUnit affectedUnit) {}

		/// "Inflict" status effect on victim 
		public virtual void OnEndTurn(AnimateUnit affectedUnit) {}
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