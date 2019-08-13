using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonEffect : Effect
{
    [SerializeField] private GameObject m_summon;
    [SerializeField] private int m_HowManyTurns;

	public override bool PerformEffect(Node _effectedNode, Unit _thisUnit)
	{
		base.PerformEffect(_effectedNode, _thisUnit);

		if (!m_isTileAllowed)
			return false;

		Transform go = Instantiate(m_summon, 
						_effectedNode.gameObject.transform.position, 
						Quaternion.identity, null).transform;
		Crystal unit = go.GetComponent<Crystal>();
		unit.m_coordinates = _effectedNode.GetCoordinates();
		unit.m_HowManyTurns = m_HowManyTurns;
		_effectedNode.SetUnitOnTop(unit);
		GameManager.GetInstance().AddCrystal(unit);

		return true;
	}
}
