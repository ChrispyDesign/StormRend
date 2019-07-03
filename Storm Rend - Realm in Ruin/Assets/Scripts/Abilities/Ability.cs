using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct AbilityLevel
{
    [TextArea]
    public string m_abilityEffect;
    public int m_gloryRequirement;
}

public enum ExecutionOrder
{
    StartOfTurn,
    EndOfTurn
}

public abstract class Ability : MonoBehaviour
{
    [SerializeField] private string m_abilityName;
    [SerializeField] private AbilityLevel m_abilityEffectLevel1;
    [SerializeField] private AbilityLevel m_abilityEffectLevel2;
    [SerializeField] private AbilityLevel m_abilityEffectLevel3;
    
    public abstract void CastImmediately();
    public abstract void CastDelayed(ExecutionOrder executionOrder, int turnDelay);
}
