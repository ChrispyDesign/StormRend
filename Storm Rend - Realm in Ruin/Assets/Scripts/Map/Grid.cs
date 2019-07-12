using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid 
{
    public static Node[,] m_nodes;

    [SerializeField] private SpawnManager m_spawnManager;

    private Transform m_parent;
    private int m_nodeSize;
    private Vector3Int m_gridSize;
    Vector2Int v;

    private NodeType[,] m_gridData;
    
    public Grid(Transform _prefab, int _nodeSize, Transform _parent, NodeType[,] _gridData)
    {
        m_parent = _parent;
        m_gridSize.x = _gridData.GetLength(0);
        m_gridSize.y = _gridData.GetLength(1);
        m_nodeSize = _nodeSize;
        m_gridData = _gridData;
        CreateGrid(_prefab);
    }

    void CreateGrid(Transform _prefab)
    {
        m_nodes = new Node[m_gridSize.x, m_gridSize.y];
        for (int x = 0; x < m_gridSize.x; x++)
        {
            for (int y = 0; y < m_gridSize.y; y++)
            {
                Vector3 pos = new Vector3( -(m_gridSize.x / 2) + x * m_nodeSize,
                                            0.0f,
                                            -(m_gridSize.y / 2) + y * m_nodeSize);
                Transform tile = Object.Instantiate(_prefab, pos, Quaternion.identity, m_parent);
                tile.name = "(" + x + ", " + y + ")";
                m_nodes[x, y] = tile.GetComponent<Node>().SetNodeVariables(pos, new Vector2Int(x, y), m_gridData[x,y]);
                
                if (m_nodes[x, y].m_nodeType == NodeType.EMPTY)
                    m_nodes[x, y].GetComponent<MeshRenderer>().enabled = false;

                if (m_nodes[x, y].m_nodeType == NodeType.BLOCKED)
                    m_nodes[x, y].GetComponent<MeshRenderer>().material.color = Color.black;

                if(m_nodes[x, y].m_nodeType == NodeType.PLAYER)
                    m_nodes[x, y].GetComponent<MeshRenderer>().material.color = Color.green;

                if (m_nodes[x, y].m_nodeType == NodeType.BERSERKER)
                {
                    m_nodes[x, y].GetComponent<MeshRenderer>().material.color = Color.green;
                    m_spawnManager.m_spawns[0].m_spawnCoords.x = x;
                    m_spawnManager.m_spawns[0].m_spawnCoords.y = y;
                }

                if (m_nodes[x, y].m_nodeType == NodeType.VALKYRIE)
                {
                    m_nodes[x, y].GetComponent<MeshRenderer>().material.color = Color.green;
                    m_spawnManager.m_spawns[1].m_spawnCoords.x = x;
                    m_spawnManager.m_spawns[1].m_spawnCoords.y = y;
                }

                if (m_nodes[x, y].m_nodeType == NodeType.SAGE)
                {
                    m_nodes[x, y].GetComponent<MeshRenderer>().material.color = Color.green;
                    m_spawnManager.m_spawns[2].m_spawnCoords.x = x;
                    m_spawnManager.m_spawns[2].m_spawnCoords.y = y;
                }

                if (m_nodes[x, y].m_nodeType == NodeType.ENEMY)
                    m_nodes[x, y].GetComponent<MeshRenderer>().material.color = Color.red;
            }
        }

        for (int x = 0; x < m_gridSize.x; x++)
        {
            for (int y = 0; y < m_gridSize.y; y++)
            {
                m_nodes[x,y].SetNeighbours(GenerateNeighbours(x, y));
            }
        }
    }

    Node[] GenerateNeighbours(int _x, int _y)
    {
        Node[] neighbours = new Node[4];
        
        if (_y < m_gridSize.y - 1)
            neighbours[(int)Neighbour.UP] = m_nodes[_x, _y + 1];

        if(_x < m_gridSize.x - 1)
            neighbours[(int)Neighbour.RIGHT] = m_nodes[_x + 1, _y];

        if (_y > 0)
            neighbours[(int)Neighbour.DOWN] = m_nodes[_x, _y - 1];

        if (_x > 0)
            neighbours[(int)Neighbour.LEFT] = m_nodes[_x - 1, _y];

        return neighbours;
    }

    public static Node GetNodeFromCoords(int _x, int _y) { return m_nodes[_x, _y]; }
    public static Node GetNodeFromCoords(Vector2Int _coords) { return m_nodes[_coords.x, _coords.y]; }
}
