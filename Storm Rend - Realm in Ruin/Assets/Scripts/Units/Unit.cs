﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour, ISelectable, IHoverable
{
    [SerializeField] private MeshRenderer m_meshRenderer = null;

    public Vector2Int m_coordinates;

    #region unit stats

    [SerializeField] private int m_maxHP = 4;
    [SerializeField] private int m_maxMOV = 4;
    private int m_HP;

    public int GetHP() { return m_HP; }
    public void SetHP(int value) { m_HP = Mathf.Clamp(value, 0, m_maxHP); }

    public int GetMove() { return m_maxMOV; }

    #endregion

    public Node GetCurrentNode() { return Grid.GetNodeFromCoords(m_coordinates); }

    // Start is called before the first frame update
    void Start()
    {
        m_HP = m_maxHP;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MoveTo(Node _moveToNode)
    {
        GetCurrentNode().SetUnitOnTop(null);
        _moveToNode.SetUnitOnTop(this);

        m_coordinates = _moveToNode.GetCoordinates();
        transform.position = _moveToNode.GetNodePosition();
        Player.SetCurrentPlayer(null);
    }

    public virtual void OnSelect()
    {
        List<Node> nodes = Dijkstra.Instance.m_validMoves;

        foreach (Node node in nodes)
        {
            if (node.GetUnitOnTop())
                continue;

            node.transform.GetComponent<MeshRenderer>().material.color = Color.red;
            node.m_selected = true;
        }
        
        Color materialColour = m_meshRenderer.material.color;
        m_meshRenderer.material.color = new Color(materialColour.r, materialColour.g, materialColour.b, 0.5f);
    }

    public virtual void OnDeselect()
    {
        Grid.GetNodeFromCoords(m_coordinates).OnDeselect();

        Color materialColour = m_meshRenderer.material.color;
        m_meshRenderer.material.color = new Color(materialColour.r, materialColour.g, materialColour.b, 1);
    }

    public virtual void OnHover()
    {

    }
    
    public virtual void OnUnhover()
    {

    }
}
