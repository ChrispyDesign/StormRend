using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeType
{
    WALKABLE,
    EMPTY,
    BLOCKED,
    PLAYER,
    ENEMY,
    NOT_ASSIGNED,

    COUNT
}

public class Map : MonoBehaviour
{
    public Grid m_grid;
    public int m_gridWorlodSize;

    [SerializeField] private Transform m_tilePrefab;

    private void Start()
    {
        Transform parent = new GameObject("Tiles").transform;
        parent.parent = this.transform;
        m_grid = new Grid(m_tilePrefab, m_gridWorlodSize, parent);
    }
}
