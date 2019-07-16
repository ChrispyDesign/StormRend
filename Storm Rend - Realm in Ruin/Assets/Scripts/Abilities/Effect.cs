using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class Effect : ScriptableObject
{
    public TargetableTiles m_canTarget;

    public bool m_isFoldOut { get; set; } = true;
}