using StormRend.MapSystems.Tiles;
using StormRend.Units;
using UnityEngine;

namespace StormRend.Abilities.Effects
{
	/// <summary>
	/// Deals reflex damage when affectunit is attacked
	/// </summary>
    public class TauntEffect : StatusEffect
    {
        [SerializeField] int reflexDamage = 1;

		public override void Perform(Ability ability, Unit owner, Tile[] targetTiles)
		{
			AddStatusEffectToAnimateUnits(targetTiles);
		}

		public override void OnBeginTurn(AnimateUnit affectedUnit)
		{
			base.OnBeginTurn(affectedUnit);		//Housekeeping
		}

		public override void OnTakeDamage(Unit affectedUnit, DamageData damageData)
		{
			//Apply reflex damage; The victim attacks back
			damageData.attacker.TakeDamage(new DamageData(affectedUnit, reflexDamage));
		}
	}
}