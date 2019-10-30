using StormRend.Defunct;
using UnityEngine;

namespace StormRend.Abilities.Effects
{
    public class GloryEffect : xEffect
    {
        [SerializeField] private int m_gloryAmount;

        public override bool PerformEffect(xTile _effectedNode, xUnit _thisUnit)
        {
            base.PerformEffect(_effectedNode, _thisUnit);

            if (!m_isTileAllowed)
                return false;

            xUIManager.GetInstance().GetGloryManager().GainGlory(m_gloryAmount);

            return true;
        }
    }
}