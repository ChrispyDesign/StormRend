using UnityEngine;

namespace StormRend.Abilities.Effects
{
    public class DamageEffect : Effect
    {
        [SerializeField] int m_damageAmount;

        public override bool PerformEffect(Tile targetTile, Unit effectPerformer)
        {
            base.PerformEffect(targetTile, effectPerformer);

            if (!m_isTileAllowed)
                return false;

            Unit unit = targetTile.GetUnitOnTop();

            if (unit != null)
                unit.TakeDamage(m_damageAmount);

            return true;
        }
    }
}
