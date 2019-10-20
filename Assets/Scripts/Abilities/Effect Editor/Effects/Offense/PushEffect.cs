using StormRend;
using UnityEngine;

public enum EffectedTile
{
	LEFT,
	RIGHT,
	UP,
	DOWN,

	COUNT
}

public class PushEffect : Effect
{
	//[SerializeField] private EffectedTile m_direction;
    [SerializeField] int m_pushAmount;
	[SerializeField] bool m_gainProtection;
	[SerializeField] bool m_doDamage;
	[SerializeField] int m_damage;

	Vector2Int m_left = new Vector2Int(-1, 0);
	Vector2Int m_up = new Vector2Int(0, 1);
	Vector2Int m_right = new Vector2Int(1, 0);
	Vector2Int m_down = new Vector2Int(0, -1);

	public override bool PerformEffect(Tile _effectedNode, Unit _thisUnit)
	{
		base.PerformEffect(_effectedNode, _thisUnit);
		
		if (!m_isTileAllowed)
			return false;

		Vector2Int nodeCoords = _effectedNode.GetCoordinates();
		Vector2Int unitCoords = _thisUnit.coords;

		if(m_gainProtection)
			_thisUnit.isProtected = true;

		Vector2Int tempCoords = new Vector2Int(0,0);
		Tile tempNode;

		// Process Left
		tempCoords = nodeCoords - m_left;
		tempNode = Grid.CoordToTile(tempCoords);

		if (tempNode.GetUnitOnTop() != null)
		{
			Unit unit = tempNode.GetUnitOnTop();
			Tile newNode = Grid.CoordToTile(tempCoords - m_left);

			if(m_doDamage)
				Grid.CoordToTile(tempCoords).GetUnitOnTop().TakeDamage(m_damage);

			if (newNode.GetUnitOnTop() == null
				&& newNode.m_nodeType != NodeType.BLOCKED)
				unit.MoveTo(newNode);
		}

		// Process Up
		tempCoords = nodeCoords - m_up;
		tempNode = Grid.CoordToTile(tempCoords);

		if (tempNode.GetUnitOnTop() != null)
		{
			Unit unit = tempNode.GetUnitOnTop();
			Tile newNode = Grid.CoordToTile(tempCoords - m_up);

			if (m_doDamage)
				Grid.CoordToTile(tempCoords).GetUnitOnTop().TakeDamage(m_damage);

			if (newNode.GetUnitOnTop() == null
				&& newNode.m_nodeType != NodeType.BLOCKED)
				unit.MoveTo(newNode);
		}

		// Process Right
		tempCoords = nodeCoords - m_right;
		tempNode = Grid.CoordToTile(tempCoords);

		if (tempNode.GetUnitOnTop() != null)
		{
			Unit unit = tempNode.GetUnitOnTop();
			Tile newNode = Grid.CoordToTile(tempCoords - m_right);

			if (m_doDamage)
				Grid.CoordToTile(tempCoords).GetUnitOnTop().TakeDamage(m_damage);

			if (newNode.GetUnitOnTop() == null
				&& newNode.m_nodeType != NodeType.BLOCKED)
				unit.MoveTo(newNode);
		}

		// Process Down
		tempCoords = nodeCoords - m_down;
		tempNode = Grid.CoordToTile(tempCoords);

		if (tempNode.GetUnitOnTop() != null)
		{
			Unit unit = tempNode.GetUnitOnTop();
			Tile newNode = Grid.CoordToTile(tempCoords - m_down);

			if (m_doDamage)
				Grid.CoordToTile(tempCoords).GetUnitOnTop().TakeDamage(m_damage);

			if (newNode.GetUnitOnTop() == null
				&& newNode.m_nodeType != NodeType.BLOCKED)
				unit.MoveTo(newNode);
		}

		return true;
	}
}