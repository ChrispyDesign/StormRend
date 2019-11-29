using StormRend.MapSystems.Tiles;
using StormRend.Units;
using UnityEngine;

namespace StormRend.Abilities.Effects
{
	/// <summary>
	/// Deals reflex damage when affectunit is attacked
	/// </summary>
	public class TauntEffect : RuneStatusEffect
	{
		[SerializeField] int reflexDamage = 1;
		[SerializeField] string inbuiltVFXName = "FX_Provoke";

		public override void Perform(Ability ability, Unit owner, Tile[] targetTiles)
		{
			AddStatusEffectToTargets(targetTiles);
		}

		public override bool OnStartTurn(AnimateUnit affectedUnit)
		{
			//Tick this effect
			var valid = base.OnStartTurn(affectedUnit);

			if (valid)
				affectedUnit.animEventHandlers.ActivateInbuiltVFX(inbuiltVFXName);
			else
				affectedUnit.animEventHandlers.DeactivateInbuiltVFX(inbuiltVFXName);
				
			return valid;
		}

		public override bool OnTakeDamage(Unit affectedUnit, HealthData damageData)
		{
			//Apply reflex damage; The victim attacks back
			damageData.vendor.TakeDamage(new HealthData(affectedUnit, reflexDamage));

			return true;
		}
	}
}