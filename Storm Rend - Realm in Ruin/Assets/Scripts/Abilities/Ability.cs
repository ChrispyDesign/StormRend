using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct AbilityLevelInfo
{
    [TextArea]
    public string m_abilityEffect;
    public Sprite m_abilityIcon;
    public int m_gloryRequirement;
}

public enum AbilityLevel
{
    LEVEL_1,
    LEVEL_2,
    LEVEL_3
}

public enum ExecutionOrder
{
    TURN_START,
    TURN_END
}

public abstract class Ability : MonoBehaviour
{
    [SerializeField] private string m_abilityName;
    [SerializeField] protected AbilityLevelInfo[] m_effectLevels;

    #region getters

    public AbilityLevelInfo GetLevel(int level) { return m_effectLevels[level]; }

    #endregion

    public abstract void CastImmediately(AbilityLevel abilityLevel);
}