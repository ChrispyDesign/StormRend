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
    [SerializeField] private int m_pushAmount;
	private Vector2Int m_left = new Vector2Int(-1, 0);
	private Vector2Int m_up = new Vector2Int(0, 1);
	private Vector2Int m_right = new Vector2Int(1, 0);
	private Vector2Int m_down = new Vector2Int(0, -1);

	public override bool PerformEffect(Node _effectedNode, Unit _thisUnit)
	{
		base.PerformEffect(_effectedNode, _thisUnit);
		
		if (!m_isTileAllowed)
			return false;

		Vector2Int nodeCoords = _effectedNode.GetCoordinates();
		Vector2Int unitCoords = _thisUnit.m_coordinates;

		Vector2Int tempCoords = new Vector2Int(0,0);
		Node tempNode;

		// Process Left
		tempCoords = nodeCoords - m_left;
		tempNode = Grid.GetNodeFromCoords(tempCoords);

		if (tempNode.GetUnitOnTop() != null)
		{
			Unit unit = tempNode.GetUnitOnTop();
			Node newNode = Grid.GetNodeFromCoords(tempCoords - m_left);
			if(newNode.GetUnitOnTop() == null)
				unit.MoveTo(newNode);
		}

		// Process Up
		tempCoords = nodeCoords - m_up;
		tempNode = Grid.GetNodeFromCoords(tempCoords);

		if (tempNode.GetUnitOnTop() != null)
		{
			Unit unit = tempNode.GetUnitOnTop();
			Node newNode = Grid.GetNodeFromCoords(tempCoords - m_up);
			if (newNode.GetUnitOnTop() == null)
				unit.MoveTo(newNode);
		}

		// Process Right
		tempCoords = nodeCoords - m_right;
		tempNode = Grid.GetNodeFromCoords(tempCoords);

		if (tempNode.GetUnitOnTop() != null)
		{
			Unit unit = tempNode.GetUnitOnTop();
			Node newNode = Grid.GetNodeFromCoords(tempCoords - m_right);
			if (newNode.GetUnitOnTop() == null)
				unit.MoveTo(newNode);
		}

		// Process Down
		tempCoords = nodeCoords - m_down;
		tempNode = Grid.GetNodeFromCoords(tempCoords);

		if (tempNode.GetUnitOnTop() != null)
		{
			Unit unit = tempNode.GetUnitOnTop();
			Node newNode = Grid.GetNodeFromCoords(tempCoords - m_down);
			if (newNode.GetUnitOnTop() == null)
				unit.MoveTo(newNode);
		}

		return true;
	}
}