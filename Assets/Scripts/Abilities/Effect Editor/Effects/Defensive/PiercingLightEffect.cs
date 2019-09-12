using System.Collections.Generic;
using UnityEngine;
using StormRend;

public class PiercingLightEffect : Effect
{
	public int m_damage;
	public int m_tilesToEffectInfront;
	public int m_gloryGainPerUnit;
	public bool m_inflictBlindness;

	public override bool PerformEffect(Tile _effectedNode, Unit _thisUnit)
	{
		Vector2Int unitCoords = _thisUnit.coords;
		Vector2Int tempCoord = _effectedNode.GetCoordinates() - unitCoords;

		List<Tile> nodesToEffect = new List<Tile>();
		nodesToEffect.Add(_effectedNode);

		UIManager uiManager = UIManager.GetInstance();
		Unit unit = _effectedNode.GetUnitOnTop();
		if (unit != null && unit.GetType() != typeof(PlayerUnit))
		{
			if (m_inflictBlindness)
				unit.m_blind = true;

			unit.TakeDamage(m_damage);

			if (unit.GetType() != typeof(PlayerUnit))
				uiManager.GetGloryManager().GainGlory(m_gloryGainPerUnit);
		}
		
		for(int i = 0; i < m_tilesToEffectInfront; i++)
		{
			Vector2Int coords = nodesToEffect[nodesToEffect.Count - 1].GetCoordinates() + tempCoord;
			Tile node = Grid.GetNodeFromCoords(coords.x, coords.y);
			nodesToEffect.Add(node);

			unit = node.GetUnitOnTop();
			if (unit != null)
			{
				if (m_inflictBlindness)
					unit.m_blind = true;

				unit.TakeDamage(m_damage);

				if(unit.GetType() != typeof(PlayerUnit))
					uiManager.GetGloryManager().GainGlory(m_gloryGainPerUnit);
			}
		}

		return true;
	}
}
