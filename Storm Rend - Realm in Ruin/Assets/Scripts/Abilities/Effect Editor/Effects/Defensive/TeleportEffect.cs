using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportEffect : Effect
{
	public override bool PerformEffect(Node _effectedNode, Unit _thisUnit)
	{
		base.PerformEffect(_effectedNode, _thisUnit);

		if (!m_isTileAllowed)
			return false;

		if (_effectedNode.GetUnitOnTop() != null)
		{
			_thisUnit.SetAlreadyAttacked(false);
			return false;
		}
		_thisUnit.MoveTo(_effectedNode);

		return true;
	}
}
