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
    [SerializeField] private Grid m_grid;
    [SerializeField] private int m_gridWorldSize;
    [SerializeField] private int m_nodeSize;
    [SerializeField] private Transform m_tilePrefab;

    private TileData m_gridData;

    private void Awake()
    {
        Transform parent = new GameObject("Tiles").transform;
        parent.parent = this.transform;
        m_grid = new Grid(m_tilePrefab, m_gridWorldSize, m_nodeSize, parent, m_gridData);
    }
}

[System.Serializable]
public class TileData
{
    [System.Serializable]
    public struct rowData
    {
        public NodeType[] row;
    }

    public rowData[] rows = new rowData[17];
}