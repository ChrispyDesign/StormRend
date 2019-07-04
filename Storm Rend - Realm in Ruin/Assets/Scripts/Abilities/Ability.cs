using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct AbilityLevelInfo
{
    [TextArea]
    public string m_abilityEffect;
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
    [SerializeField] protected AbilityLevelInfo m_effectLevel1;
    [SerializeField] protected AbilityLevelInfo m_effectLevel2;
    [SerializeField] protected AbilityLevelInfo m_effectLevel3;
    
    public abstract void CastImmediately(AbilityLevel abilityLevel);
    public abstract void CastDelayed(ExecutionOrder executionOrder, int turnDelay, int level = 1);
}