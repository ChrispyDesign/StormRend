using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Neighbour
{
    UP,
    RIGHT,
    DOWN,
    LEFT
}

public class Node
{
    public Transform m_unitOnTop;
    public Node[] m_neighbours;
    public Vector3 m_position;
    public Vector2Int m_coordinate;
    public NodeType m_nodeType;

    public Node(Transform _unitOnTop, Vector3 _pos, Vector2Int _coordinate, NodeType _nodeType, Transform _prefab)
    {
        m_unitOnTop = _unitOnTop;
        m_neighbours = new Node[4];
        m_position = _pos;
        m_coordinate = _coordinate;
        m_nodeType = _nodeType;
        GameObject.Instantiate(_prefab, m_position, Quaternion.identity);
    }

    List<Node> GetNeighbours()
    {
        List<Node> neighbours = new List<Node>();

        foreach (Node node in m_neighbours)
        {
            if (node == null)
                break;

            neighbours.Add(node);
        }
        return neighbours;
    }
}
