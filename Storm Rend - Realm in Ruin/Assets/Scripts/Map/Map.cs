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
    [SerializeField] private int m_gridWorlodSize;
    [SerializeField] private int nodeSize;
    [SerializeField] public MyGrid f;

    [SerializeField] private Transform m_tilePrefab;

    private void Start()
    {
        f = new MyGrid(10, 10);
        Transform parent = new GameObject("Tiles").transform;
        parent.parent = this.transform;
        m_grid = new Grid(m_tilePrefab, m_gridWorlodSize, nodeSize, parent);
    }
}

[System.Serializable]
public class MyGrid : Serializable2DArray<NodeType>
{
    public MyGrid(int aCols, int aRows) : base(aCols, aRows)
    {

    }
}

[System.Serializable]
public class Serializable2DArray<T>
{
    private int m_Columns = 1;
    [SerializeField]
    private T[] m_Data;
    
    public T this[int aCol, int aRow]
    {
        get { return m_Data[aRow * m_Columns + aCol]; }
        set { m_Data[aRow * m_Columns + aCol] = value; }
    }
    public int Columns { get { return m_Columns; } }
    public int Rows { get { return m_Data.Length / m_Columns; } }
    public Serializable2DArray(int aCols, int aRows)
    {
        m_Columns = aCols;
        m_Data = new T[aCols * aRows];
    }
}

