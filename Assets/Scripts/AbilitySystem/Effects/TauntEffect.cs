using StormRend.MapSystems.Tiles;
using StormRend.Units;
using UnityEngine;

namespace StormRend.Abilities.Effects
{
    public class TauntEffect : StatusEffect
    {
		/* Brainstorm:
		- When an unit attacks a unit that has taunt cast on him, the enemy gets damage
		- Could put this in takedamage
		 */
        [SerializeField] int reflexDamage = 1;

		public override void Perform(Unit owner, Tile[] targetTiles)
		{
			AddStatusEffectToAnimateUnits(targetTiles);
		}

		public override void OnTakeDamage(Unit victim, Unit attacker)
		{
			//Apply reflex damage; The victim attacks back
			attacker.TakeDamage(new DamageData(victim, reflexDamage));
		}
	}
}