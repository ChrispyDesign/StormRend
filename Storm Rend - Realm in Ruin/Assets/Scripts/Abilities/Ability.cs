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

[System.Serializable]
public struct TargetableTiles
{
    public bool m_empty;
    public bool m_enemies;
    public bool m_players;
    public bool m_self;
}

[System.Serializable]
public struct Effects
{
    public bool m_damage;
    public bool m_heal;
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
    public string m_name;
    public Sprite m_icon = null;
    public string m_description;
    
    public int m_gloryRequirement = 0;
    public int m_tilesToSelect = 1;

    public RowData[] m_castArea = new RowData[7];
    public TargetableTiles m_targetableTiles;
    public List<Effect> m_effects = new List<Effect>();

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