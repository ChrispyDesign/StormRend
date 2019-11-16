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
		[SerializeField] string inbuiltVFXName = "FX_Provoke";

		public override void Perform(Ability ability, Unit owner, Tile[] targetTiles)
		{
			AddStatusEffectToAnimateUnits(targetTiles);
		}

		public override void OnBeginTurn(AnimateUnit affectedUnit)
		{
			base.OnBeginTurn(affectedUnit);     //Housekeeping

			if (affectedTurns > 0 
				&& turnCount >= affectedTurns)
			{
				//HARDCODE If this effect has expired then also deactivate relevant VFX
				affectedUnit.animEventHandlers.DeactivateInbuiltVFX(inbuiltVFXName);
			}
		}

		public override void OnTakeDamage(Unit affectedUnit, HealthData damageData)
		{
			//Apply reflex damage; The victim attacks back
			damageData.vendor.TakeDamage(new HealthData(affectedUnit, reflexDamage));
		}
	}
}