using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour, ISelectable, IHoverable
{
    [SerializeField] private MeshRenderer m_meshRenderer = null;
    [SerializeField] private bool m_debug;

    public Vector2Int m_coordinates;

    #region unit stats

    [SerializeField] private int m_maxHP = 4;
    [SerializeField] private int m_maxMOV = 4;
    private int m_HP;

    public int GetHP() { return m_HP; }
    public void SetHP(int value) { m_HP = Mathf.Clamp(value, 0, m_maxHP); }

    public int GetMove() { return m_maxMOV; }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        m_HP = m_maxHP;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public virtual void OnSelect()
    {
        if(m_debug)
            m_meshRenderer.material.color = Color.blue;

        Dijkstra.Instance.FindValidMoves(Grid.GetNodeFromCoords(m_coordinates), GetMove());
        List<Node> nodes = Dijkstra.Instance.m_validMoves;
        foreach (Node node in nodes)
        {
            node.transform.GetComponent<MeshRenderer>().material.color = Color.red;
            node.m_selected = true;
        }
    }

    public virtual void OnDeselect()
    {
        if (m_debug)
            m_meshRenderer.material.color = Color.white;

        Grid.GetNodeFromCoords(m_coordinates).OnDeselect();
    }

    public virtual void OnHover()
    {
        if (m_debug)
            m_meshRenderer.material.color = Color.green;
    }
    
    public virtual void OnUnhover()
    {
        if (m_debug)
            m_meshRenderer.material.color = Color.white;
    }
}
