using StormRend.Defunct;
using UnityEngine;

namespace StormRend.Abilities.Effects
{
    public class DamageEffect : xEffect
    {
        [SerializeField] int m_damageAmount;

        public override bool PerformEffect(xTile targetTile, xUnit effectPerformer)
        {
            base.PerformEffect(targetTile, effectPerformer);

            if (!m_isTileAllowed)
                return false;

            xUnit unit = targetTile.GetUnitOnTop();

            if (unit != null)
                unit.TakeDamage(m_damageAmount);

            return true;
        }
    }
}
