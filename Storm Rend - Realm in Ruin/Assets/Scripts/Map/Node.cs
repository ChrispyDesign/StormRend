﻿using System.Collections;
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

    private Color m_origMaterial;

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
        m_origMaterial = transform.GetComponent<MeshRenderer>().material.color;
        transform.GetComponent<MeshRenderer>().material.color = Color.red;

        PlayerUnit currentSelectedUnit = PlayerController.GetCurrentPlayer();
        if (currentSelectedUnit && !m_unitOnTop && currentSelectedUnit.GetAvailableNodes().Contains(this))
        {
            currentSelectedUnit.MoveDuplicateTo(this);
        }
    }

    public void OnUnhover()
    {
        if (!m_selected)
            transform.GetComponent<MeshRenderer>().material.color = Color.white;

        transform.GetComponent<MeshRenderer>().material.color = m_origMaterial;
    }

    public void OnSelect()
    {
        PlayerUnit currentSelectedUnit = PlayerController.GetCurrentPlayer();

        if (currentSelectedUnit.GetAttackNodes() != null &&
            currentSelectedUnit.GetAttackNodes().Count > 0)
            currentSelectedUnit.UnShowAttackTiles();

        if (PlayerController.GetCurrentMode() == PlayerMode.MOVE)
        { 
            if (currentSelectedUnit && currentSelectedUnit.GetIsFocused())
            {
                List<Node> nodes = currentSelectedUnit.GetAvailableNodes();

                if (nodes.Contains(this) && !m_unitOnTop)
                {
                    if (currentSelectedUnit.GetAlreadyMoved())
                    {
                        currentSelectedUnit.GetMoveCommand().SetCoordinates(m_coordinate);
                        currentSelectedUnit.GetMoveCommand().Execute();
                    }
                    else
                    {
                        currentSelectedUnit.SetMoveCommand(new MoveCommand(
                                                               currentSelectedUnit,
                                                               m_coordinate));
                        MoveCommand temp = currentSelectedUnit.GetMoveCommand();
                        temp.Execute();

                        CommandManager.m_moves.Add(temp);
                    }

                    FindObjectOfType<Camera>().GetComponent<CameraMove>().MoveTo(transform.position, 0.5f);
                }
                currentSelectedUnit.SetDuplicateMeshVisibilty(false);
                currentSelectedUnit.SetIsFocused(false);
            }
        }

        if (PlayerController.GetCurrentMode() == PlayerMode.ATTACK)
        {
            Ability ability = currentSelectedUnit.GetLockedAbility();
            foreach(Effect effect in ability.GetEffects())
            {
                effect.PerformEffect(this);
            }
        }
    }

    public void OnDeselect()
    {
        m_selected = false;

        Unit unitOnTop = GetUnitOnTop();
        if (unitOnTop)
        {
            List<Node> nodes = unitOnTop.GetAvailableNodes();

            foreach (Node node in nodes)
            {
                node.transform.GetComponent<MeshRenderer>().material.color = Color.white;
                node.m_selected = false;
            }
        }

    }
}