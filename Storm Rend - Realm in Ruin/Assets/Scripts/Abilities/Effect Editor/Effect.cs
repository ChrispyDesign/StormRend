using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum Target
{
    SelectedTiles,
    SelectedTilesWithBreadth,
    AdjacentTiles,
    Self
}

[System.Serializable]
public class Effect : ScriptableObject
{
    public Target m_target;
    public UnityEvent OnPeformEffect;

	protected bool m_isTileAllowed;

    public bool m_isFoldOut { get; set; } = true;

    public virtual void PerformEffect(Node _effectedNode, Unit _thisUnit)
    {
        OnPeformEffect.Invoke();

		Ability ability = _thisUnit.GetLockedAbility();
		TargetableTiles tileInfo = ability.GetTargetableTiles();

		if (tileInfo.m_empty == (_effectedNode.GetUnitOnTop() == null))
			m_isTileAllowed = true;

		if (_effectedNode.GetUnitOnTop() != null)
		{
			if (tileInfo.m_enemies && 
				tileInfo.m_enemies == (_effectedNode.GetUnitOnTop().GetComponent<EnemyUnit>() != null))
				m_isTileAllowed = true;

			if (tileInfo.m_players && 
				tileInfo.m_players == (_effectedNode.GetUnitOnTop().GetComponent<PlayerUnit>() != null))
				m_isTileAllowed = true;

			if (tileInfo.m_self && 
				tileInfo.m_self == (_effectedNode.GetUnitOnTop().GetComponent<Unit>() == _thisUnit))
				m_isTileAllowed = true;
		}

		if (!m_isTileAllowed)
		{
			_thisUnit.SetAlreadyAttacked(false);
			return;
		}

		_thisUnit.SetAlreadyMoved(true);
		_thisUnit.SetAlreadyAttacked(true);

		UIManager.GetInstance().GetGloryManager().SpendGlory(ability.GetGloryRequirement());
    }
}