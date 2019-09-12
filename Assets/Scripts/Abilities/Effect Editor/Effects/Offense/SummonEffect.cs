using System.Collections;
using System.Collections.Generic;
using StormRend;
using UnityEngine;

public class SummonEffect : Effect
{
	public bool m_inflictCrippling;
    [SerializeField] GameObject m_summon;
    [SerializeField] int m_HowManyTurns;

	public override bool PerformEffect(Tile _effectedNode, Unit _thisUnit)
	{
		base.PerformEffect(_effectedNode, _thisUnit);

		if (!m_isTileAllowed)
			return false;

		Transform go = Instantiate(m_summon, 
						_effectedNode.gameObject.transform.position, 
						Quaternion.identity, null).transform;
		Crystal unit = go.GetComponent<Crystal>();
		unit.coords = _effectedNode.GetCoordinates();
		unit.m_HowManyTurns = m_HowManyTurns;
		unit.m_inflictCrippling = m_inflictCrippling;
		_effectedNode.SetUnitOnTop(unit);
		GameManager.singleton.AddCrystal(unit);

		return true;
	}
}
