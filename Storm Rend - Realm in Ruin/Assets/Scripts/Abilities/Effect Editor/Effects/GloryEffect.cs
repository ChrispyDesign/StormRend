using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GloryEffect : Effect
{
    [SerializeField] private int m_gloryAmount;

	public override bool PerformEffect(Node _effectedNode, Unit _thisUnit)
	{
		base.PerformEffect(_effectedNode, _thisUnit);

		if (!m_isTileAllowed)
			return false;

		UIManager.GetInstance().GetGloryManager().GainGlory(m_gloryAmount);

		return true;
	}
}
