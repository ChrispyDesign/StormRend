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

    private bool m_selected = false;

    public NodeType m_nodeType;

    public Node m_parent;
    public int m_nGCost, m_nHCost;
    public int m_nFCost
    {
        get
        {
            return m_nGCost + m_nHCost;
        }
    }

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

    public List<Node> GetNeighbours()
    {
        List<Node> neighbours = new List<Node>();

        foreach (Node node in m_neighbours)
        {
            if (node == null)
                continue;

            neighbours.Add(node);
        }
        return neighbours;
    }

    public void OnHover()
    {
        transform.GetComponent<MeshRenderer>().material.color = Color.red;
    }

    public void OnUnhover()
    {
        if (!m_selected)
            transform.GetComponent<MeshRenderer>().material.color = Color.black;
    }

    public void OnSelect()
    {
        if (!m_selected)
        {
            m_selected = true;
            Dijkstra.Instance.FindValidMoves(this, 2);
            List<Node> nodes = Dijkstra.Instance.m_validMoves;

            foreach (Node n in nodes)
            {
                n.transform.GetComponent<MeshRenderer>().material.color = Color.red;
                n.m_selected = true;
            }
        }
    }

    public void OnDeselect()
    {
        m_selected = false;
        List<Node> nodes = Dijkstra.Instance.m_validMoves;
        foreach (Node n in nodes)
        {
            n.transform.GetComponent<MeshRenderer>().material.color = Color.black;
            n.m_selected = false;
        }
    }
}