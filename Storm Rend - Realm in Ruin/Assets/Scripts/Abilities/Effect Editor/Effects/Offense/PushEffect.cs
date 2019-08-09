using UnityEngine;

public class PushEffect : Effect
{
    [SerializeField] private int m_pushAmount;

	public override void PerformEffect(Node _effectedNode, Unit _thisUnit)
	{
		base.PerformEffect(_effectedNode, _thisUnit);
		
		if (!m_isTileAllowed)
			return;

		Vector2 nodeCoords = _effectedNode.GetCoordinates();
		Vector2 unitCoords = _thisUnit.m_coordinates;
	}
}