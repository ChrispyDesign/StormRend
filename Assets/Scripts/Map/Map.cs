using System.Collections;
using System.Collections.Generic;
using StormRend.Settings;
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

[RequireComponent(typeof(GridImporter))]
public class Map : MonoBehaviour
{
    [SerializeField] Grid m_grid;
    [SerializeField] float tileSize;
    [SerializeField] Transform m_tilePrefab;

    private TileData m_gridData;

    GameSettings settings;

    void Awake()
    {
        settings = GameSettings.singleton;
    }

    void Start()
    {
        Transform parent = new GameObject("Tiles").transform;
        parent.parent = this.transform;
        GridImporter import = GetComponent<GridImporter>();
        m_grid = new Grid(m_tilePrefab, settings.tileSize, parent, import.ImportGrid(import.m_path));
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