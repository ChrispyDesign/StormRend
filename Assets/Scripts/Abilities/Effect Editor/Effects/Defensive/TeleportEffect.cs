using System.Collections;
using System.Collections.Generic;
using StormRend;
using UnityEngine;

public class TeleportEffect : Effect
{
	public override bool PerformEffect(Tile _effectedNode, Unit _thisUnit)
	{
		base.PerformEffect(_effectedNode, _thisUnit);

		if (!m_isTileAllowed)
			return false;

		if (_effectedNode.GetUnitOnTop() != null)
		{
			_thisUnit.SetHasAttacked(false);
			return false;
		}
		_thisUnit.MoveTo(_effectedNode);

		return true;
	}
}
