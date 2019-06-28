﻿using System.Collections;
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
    [SerializeField] private int m_gridWorlodSize;
    [SerializeField] private int nodeSize;
    [SerializeField] private TileData m_gridData;

    [SerializeField] private Transform m_tilePrefab;

    private void Start()
    {
        Transform parent = new GameObject("Tiles").transform;
        parent.parent = this.transform;
        m_grid = new Grid(m_tilePrefab, m_gridWorlodSize, nodeSize, parent, m_gridData);
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

