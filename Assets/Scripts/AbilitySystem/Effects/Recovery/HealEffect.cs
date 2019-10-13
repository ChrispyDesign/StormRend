using StormRend.Defunct;
using UnityEngine;

namespace StormRend.Abilities.Effects
{
    public class HealEffect : xEffect
    {
        [SerializeField] int m_healAmount;

        public override bool PerformEffect(xTile _effectedNode, xUnit _thisUnit)
        {
            base.PerformEffect(_effectedNode, _thisUnit);

            if (!m_isTileAllowed)
                return false;

            xUnit unit = _effectedNode.GetUnitOnTop();

            if (unit != null)
            {
                unit.HP += m_healAmount;
            }

            return true;
        }
    }
}