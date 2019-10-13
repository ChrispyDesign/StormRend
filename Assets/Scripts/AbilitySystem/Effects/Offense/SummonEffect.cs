using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using StormRend.Defunct;

namespace StormRend.Abilities.Effects
{
    public class SummonEffect : xEffect
    {
        [SerializeField] GameObject m_summon;
        [SerializeField] int m_HowManyTurns;

        public override bool PerformEffect(xTile _effectedNode, xUnit _thisUnit)
        {
            base.PerformEffect(_effectedNode, _thisUnit);

            if (!m_isTileAllowed)
                return false;

            Transform go = Instantiate(m_summon,
                            _effectedNode.gameObject.transform.position,
                            Quaternion.identity, null).transform;
            xCrystal unit = go.GetComponent<xCrystal>();
            unit.coords = _effectedNode.GetCoordinates();
            unit.m_HowManyTurns = m_HowManyTurns;
            _effectedNode.SetUnitOnTop(unit);
            xGameManager.singleton.AddCrystal(unit);

            return true;
        }
    }
}