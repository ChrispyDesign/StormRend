using StormRend.MapSystems.Tiles;
using StormRend.Units;

namespace StormRend.Abilities.Effects
{
	public class ProtectEffect : StatusEffect
    {
		public override void Perform(Ability ability, Unit owner, Tile[] targetTiles)
		{
			AddStatusEffectToAnimateUnits(targetTiles);
		}

		public override bool OnBeginTurn(AnimateUnit affectedUnit)
		{
			return base.OnBeginTurn(affectedUnit);		//Housekeeping
		}

		public override void OnTakeDamage(Unit affectedUnit, HealthData damageData)
		{
			//Reverse the damage done
			affectedUnit.HP += damageData.amount;

			//Play some kind of protect effect/animation?
			affectedUnit.animator.ResetTrigger("HitReact");		//Prevent HitReact animation from playing
			affectedUnit.animator.SetTrigger("Parry");		//Maybe play some kind of block?
		}
	}
}