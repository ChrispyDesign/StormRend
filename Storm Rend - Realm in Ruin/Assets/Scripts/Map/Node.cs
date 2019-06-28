using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Neighbour
{
    UP = 0,
    RIGHT,
    DOWN,
    LEFT
}

public class Node : MonoBehaviour, IHoverable, ISelectable
{
    [SerializeField] private Transform m_unitOnTop;
    [SerializeField] private Node[] m_neighbours;
    [SerializeField] private Vector3 m_position;
    [SerializeField] private Vector2Int m_coordinate;
    public NodeType m_nodeType;

    public Node SetNodeVariables(Vector3 _pos, Vector2Int _coordinate, NodeType _nodeType)
    {
        m_neighbours = new Node[4];
        m_position = _pos;
        m_coordinate = _coordinate;
        m_nodeType = _nodeType;
        return this;
    }

    public void SetUnitOnTop(Transform _unit) { m_unitOnTop = _unit; }
    public void SetNeighbours(Node[] _neighbours) { m_neighbours = _neighbours; }

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

    public void OnHover()
    {
        
    }

    public void OnUnhover()
    {

    }

    public void OnSelect()
    {

    }

    public void OnDeselect()
    {

    }
}
