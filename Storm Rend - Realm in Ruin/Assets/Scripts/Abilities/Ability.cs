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

public enum TargetableTiles
{
    Empty,
    Enemy,
    Player,
    Self
}

public enum AbilityEffects
{
    Damage,
    Glory,
    Heal,
    Push,
    Move,
}

[System.Serializable]
public class RowData
{
    public bool[] elements = new bool[7];
}

[System.Serializable]
[CreateAssetMenu(fileName = "New Ability", menuName = "StormRend/Ability")]
public class Ability : ScriptableObject
{
    [Header("Ability Info")]
    [SerializeField] private string m_name;
    [SerializeField] private Sprite m_icon = null;
    [TextArea]
    [SerializeField] private string m_description;

    [Header("Ability Properties")]
    [SerializeField] private int m_gloryRequirement = 0;
    [SerializeField] private int m_tilesToSelect = 1;

    [HideInInspector] public RowData[] m_castArea = new RowData[7];
    [HideInInspector] public int m_targetableTileMask;
    [HideInInspector] public int m_effectMask;

    /// <summary>
    /// /////////////////////////////////////////////////////////
    /// </summary>
    [HideInInspector]
    [SerializeField] protected AbilityLevelInfo[] m_effectLevels;

    #region getters

    public AbilityLevelInfo GetLevel(int level) { return m_effectLevels[level]; }

    #endregion

    public void CastImmediately(AbilityLevel abilityLevel) { }
}