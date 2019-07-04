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
    [SerializeField] private Unit m_unitOnTop;
    [SerializeField] private Node[] m_neighbours;
    [SerializeField] private Vector3 m_position;
    [SerializeField] private Vector2Int m_coordinate;

    public bool m_selected = false;

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

    #region GettersAndSetters
    
    public Unit GetUnitOnTop() { return m_unitOnTop; }
    public Vector2Int GetCoordinates() { return m_coordinate; }
    public Vector3 GetNodePosition() { return m_position; }

    public void SetUnitOnTop(Unit _unit) { m_unitOnTop = _unit; }
    public void SetNeighbours(Node[] _neighbours) { m_neighbours = _neighbours; }

    #endregion

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
        PlayerUnit currentSelectedUnit = Player.GetCurrentPlayer();

        if (currentSelectedUnit)
        {
            List<Node> nodes = Dijkstra.Instance.m_validMoves;

            if (nodes.Contains(this) && !m_unitOnTop)
            {
                currentSelectedUnit.MoveTo(this);
            }
        }
    }

    public void OnDeselect()
    {
        m_selected = false;
        List<Node> nodes = Dijkstra.Instance.m_validMoves;
        foreach (Node node in nodes)
        {
            node.transform.GetComponent<MeshRenderer>().material.color = Color.black;
            node.m_selected = false;
        }
    }
}