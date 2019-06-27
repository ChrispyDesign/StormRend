using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "Grid")]
public class Grid 
{
    public Node[,] m_nodes;
    public int m_gridWorlodSize;
    public Transform m_prefab;
    
    public Grid()
    {
        m_nodes = new Node[m_gridWorlodSize, m_gridWorlodSize];
        CreateGrid();
    }

    void CreateGrid()
    {
        for (int x = 0; x < m_gridWorlodSize; x++)
        {
            for (int y = 0; y < m_gridWorlodSize; y++)
            {
                Vector3 pos = new Vector3(
                                            -(m_gridWorlodSize / 2) + x,
                                            0.0f,
                                            -(m_gridWorlodSize / 2) + y);
                m_nodes[x, y] = new Node(null, pos, new Vector2Int(x, y), NodeType.WALKABLE, m_prefab);
            }
        }
    }

    Node[] GenerateNeighbours(Vector2Int _coor)
    {
        Node[] neighbours = new Node[4];

        return neighbours;
    }
}
