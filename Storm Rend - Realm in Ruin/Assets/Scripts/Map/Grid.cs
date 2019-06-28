using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "Grid")]
public class Grid 
{
    public Node[,] m_nodes;

    private Transform m_parent;
    private int m_gridSize;
    
    public Grid(Transform _prefab, int _worldSize, Transform _parent)
    {
        m_parent = _parent;
        m_gridSize = _worldSize;
        CreateGrid(_prefab);
    }

    void CreateGrid(Transform _prefab)
    {
        m_nodes = new Node[m_gridSize, m_gridSize];
        for (int x = 0; x < m_gridSize; x++)
        {
            for (int y = 0; y < m_gridSize; y++)
            {
                Vector3 pos = new Vector3(
                                            -(m_gridSize / 2) + x,
                                            0.0f,
                                            -(m_gridSize / 2) + y);
                Transform tile = GameObject.Instantiate(_prefab, pos, Quaternion.identity, m_parent);
                m_nodes[x, y] = tile.GetComponent<Node>().SetNodeVariables(pos, new Vector2Int(x, y), NodeType.WALKABLE);
            }
        }

        for (int x = 0; x < m_gridSize; x++)
        {
            for (int y = 0; y < m_gridSize; y++)
            {
                m_nodes[x,y].SetNeighbours(GenerateNeighbours(x, y));
            }
        }
    }

    Node[] GenerateNeighbours(int _x, int _y)
    {
        Node[] neighbours = new Node[4];
        
        if (_y < m_gridSize - 1)
            neighbours[(int)Neighbour.UP] = m_nodes[_x, _y + 1];

        if(_x < m_gridSize - 1)
            neighbours[(int)Neighbour.RIGHT] = m_nodes[_x + 1, _y];

        if (_y > 0)
            neighbours[(int)Neighbour.DOWN] = m_nodes[_x, _y - 1];

        if (_x > 0)
            neighbours[(int)Neighbour.LEFT] = m_nodes[_x - 1, _y];

        return neighbours;
    }
}
