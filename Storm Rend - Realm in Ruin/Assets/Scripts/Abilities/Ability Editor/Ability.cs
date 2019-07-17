using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
[System.Serializable]
public struct TargetableTiles
{
    public bool m_empty;
    public bool m_enemies;
    public bool m_players;
    public bool m_self;
}

/// <summary>
/// 
/// </summary>
[System.Serializable]
public class RowData
{
    public bool[] elements = new bool[7];
}

/// <summary>
/// 
/// </summary>
[System.Serializable]
[CreateAssetMenu(fileName = "New Ability", menuName = "StormRend/Ability")]
public class Ability : ScriptableObject
{
    // ability info (for UI display purposes)
    [SerializeField] private string m_name;
    [SerializeField] private Sprite m_icon = null;
    [TextArea]
    [SerializeField] private string m_description;

    //
    [SerializeField] private int m_gloryRequirement = 0;
    [SerializeField] private int m_tilesToSelect = 1;

    public RowData[] m_castArea = new RowData[7];
    [SerializeField] private TargetableTiles m_targetableTiles;
    [SerializeField] private List<Effect> m_effects = new List<Effect>();

    #region getters

    public string GetName() { return m_name; }
    public Sprite GetIcon() { return m_icon; }
    public string GetDescription() { return m_description; }

    /// <summary> the amount of glory required to perform this ability </summary>
    public int GetGloryRequirement() { return m_gloryRequirement; }

    /// <summary> the amount of tiles to select before this ability is cast </summary>
    public int GetTilesToSelect() { return m_tilesToSelect; }
    public TargetableTiles GetTargetableTiles() { return m_targetableTiles; }
    public List<Effect> GetEffects() { return m_effects; }

    #endregion
}