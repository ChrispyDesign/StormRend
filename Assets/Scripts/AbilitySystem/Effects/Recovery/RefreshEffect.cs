using StormRend.Defunct;
using UnityEngine;

namespace StormRend.Abilities.Effects
{
    public enum RefreshType
    {
        AttackAgain,
        MoveAgain
    }

    public class RefreshEffect : xEffect
    {
        [SerializeField] RefreshType m_refreshType;

        public override bool PerformEffect(xTile _effectedNode, xUnit _thisUnit)
        {
            base.PerformEffect(_effectedNode, _thisUnit);

            if (!m_isTileAllowed)
                return false;

            _thisUnit.SetHasMoved(false);
            _thisUnit.SetHasAttacked(false);
            _thisUnit.m_afterClear = false;

            return true;
        }
    }
}