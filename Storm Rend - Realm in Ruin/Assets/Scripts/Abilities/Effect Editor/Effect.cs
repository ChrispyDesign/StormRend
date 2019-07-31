using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public bool m_isFoldOut { get; set; } = true;

    public virtual void PerformEffect(Node _effectedNode) { }
}