using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "Grid")]
public class Grid 
{
    public Node[,] m_nodes;

    private Transform m_parent;
    
    public Grid(Transform _prefab, int _worldSize, Transform _parent)
    {
        m_parent = _parent;
        CreateGrid(_prefab, _worldSize);
    }

    void CreateGrid(Transform _prefab, int _size)
    {
        m_nodes = new Node[_size, _size];
        for (int x = 0; x < _size; x++)
        {
            for (int y = 0; y < _size; y++)
            {
                Vector3 pos = new Vector3(
                                            -(_size / 2) + x,
                                            0.0f,
                                            -(_size / 2) + y);
                Transform tile = GameObject.Instantiate(_prefab, pos, Quaternion.identity, m_parent);
                m_nodes[x, y] = tile.GetComponent<Node>().SetNodeVariables(pos, new Vector2Int(x, y), NodeType.WALKABLE);
            }
        }
    }

    Node[] GenerateNeighbours(Vector2Int _coor)
    {
        Node[] neighbours = new Node[4];

        return neighbours;
    }
}
