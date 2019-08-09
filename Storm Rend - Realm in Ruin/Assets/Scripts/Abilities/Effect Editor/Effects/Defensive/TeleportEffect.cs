using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportEffect : Effect
{
	public override void PerformEffect(Node _effectedNode, Unit _thisUnit)
	{
		base.PerformEffect(_effectedNode, _thisUnit);

		if (!m_isTileAllowed)
			return;

		_thisUnit.MoveTo(_effectedNode);
	}
}
