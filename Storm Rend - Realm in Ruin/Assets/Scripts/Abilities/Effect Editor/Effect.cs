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

    public bool m_isFoldOut { get; set; } = true;

    public virtual void PerformEffect(Node _effectedNode, Unit _thisUnit)
    {
        OnPeformEffect.Invoke();
		Ability ability = _thisUnit.GetLockedAbility();
		UIManager.GetInstance().GetGloryManager().SpendGlory(ability.GetGloryRequirement());
    }
}