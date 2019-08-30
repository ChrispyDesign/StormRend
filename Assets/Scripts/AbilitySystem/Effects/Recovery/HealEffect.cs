using UnityEngine;

namespace StormRend.Abilities.Effects
{
    public class HealEffect : Effect
    {
        [SerializeField] int m_healAmount;

        public override bool PerformEffect(Tile _effectedNode, Unit _thisUnit)
        {
            base.PerformEffect(_effectedNode, _thisUnit);

            if (!m_isTileAllowed)
                return false;

            Unit unit = _effectedNode.GetUnitOnTop();

            if (unit != null)
            {
                unit.HP += m_healAmount;
            }

            return true;
        }
    }
}