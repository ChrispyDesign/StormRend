using System.Collections;
using System.Collections.Generic;
using StormRend;
using UnityEngine;

public class Crystal : Unit
{
	[HideInInspector]
	public int m_HowManyTurns;

	public void IterateTurns()
	{
		m_HowManyTurns--;

		if (m_HowManyTurns <= 0)
			Die();

		foreach (Effect effect in m_passiveAbility.GetEffects())
		{
			m_lockedAbility = m_passiveAbility;
			effect.PerformEffect(Grid.CoordToTile(m_coordinates), this);
		}
	}
}
