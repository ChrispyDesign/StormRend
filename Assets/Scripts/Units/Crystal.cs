using System.Collections;
using System.Collections.Generic;
using StormRend;
using UnityEngine;

public class Crystal : Unit
{
	[HideInInspector]
	public int m_HowManyTurns;

	[SerializeField] private int m_damage;

	Vector2Int m_left = new Vector2Int(-1, 0);
	Vector2Int m_up = new Vector2Int(0, 1);
	Vector2Int m_right = new Vector2Int(1, 0);
	Vector2Int m_down = new Vector2Int(0, -1);

	public void IterateTurns()
	{
		m_HowManyTurns--;

		if (m_HowManyTurns <= 0)
			Die();

		//foreach (Effect effect in m_passiveAbility.GetEffects())
		//{
		//	m_lockedAbility = m_passiveAbility;
		//	effect.PerformEffect(Grid.GetNodeFromCoords(m_coordinates), this);
		//}

		Vector2Int unitCoords = this.m_coordinates;

		Vector2Int tempCoords = new Vector2Int(0, 0);
		Tile tempNode;

		// Process Left
		tempCoords = m_coordinates - m_left;
		tempNode = Grid.GetNodeFromCoords(tempCoords);
		if(tempNode.GetUnitOnTop() != null)
			tempNode.GetUnitOnTop().TakeDamage(m_damage);

		// Process Up
		tempCoords = m_coordinates - m_up;
		tempNode = Grid.GetNodeFromCoords(tempCoords);
		if (tempNode.GetUnitOnTop() != null)
			tempNode.GetUnitOnTop().TakeDamage(m_damage);

		// Process Right
		tempCoords = m_coordinates - m_right;
		tempNode = Grid.GetNodeFromCoords(tempCoords);
		if (tempNode.GetUnitOnTop() != null)
			tempNode.GetUnitOnTop().TakeDamage(m_damage);

		// Process Down
		tempCoords = m_coordinates - m_down;
		tempNode = Grid.GetNodeFromCoords(tempCoords);
		if (tempNode.GetUnitOnTop() != null)
			tempNode.GetUnitOnTop().TakeDamage(m_damage);

	}
}
