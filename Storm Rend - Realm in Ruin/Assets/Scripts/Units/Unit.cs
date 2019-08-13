using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Unit : MonoBehaviour, ISelectable, IHoverable
{
	private int m_HP;
	private static bool m_isDead;

    [SerializeField] private GameObject m_duplicateMesh = null;
    [SerializeField] private Ability m_passiveAbility;
    [SerializeField] private Ability[] m_firstAbilities;
    [SerializeField] private Ability[] m_secondAbilities;

    [Header("Unit Stats")]
    [SerializeField] private int m_maxHP = 4;
    [SerializeField] private int m_maxMOV = 4;

    
    [Space]
    [Header("Unit Interaction")]
    [SerializeField] private UnityEvent m_onSelect;
    [SerializeField] private UnityEvent m_onDeselect;
    [SerializeField] private UnityEvent m_onHover;
    [SerializeField] private UnityEvent m_onUnhover;
	

	public Vector2Int m_coordinates;
    public bool m_afterClear;
    protected bool m_alreadyMoved;
    protected bool m_alreadyAttacked;
    protected bool m_isFocused;
    protected Ability m_lockedAbility;
    private List<Node> m_availableNodes;
    private List<Node> m_attackNodes;
   
	public Action OnDie = delegate
	{
		m_isDead = false;
	};
    #region getters

    public List<Node> GetAvailableNodes() { return m_availableNodes; }
    public Ability GetLockedAbility() { return m_lockedAbility; }
    public List<Node> GetAttackNodes() { return m_attackNodes; }
    public Node GetCurrentNode() { return Grid.GetNodeFromCoords(m_coordinates); }
    public int GetMaxHP() { return m_maxHP; }
    public int GetHP() { return m_HP; }
    public int GetMove() { return m_maxMOV; }
    public bool GetIsFocused() { return m_isFocused; }
    public bool GetAlreadyMoved() { return m_alreadyMoved; }
    public bool GetAlreadyAttacked() { return m_alreadyAttacked; }
	public bool GetIsDead() { return m_isDead; }

    public void GetAbilities( ref Ability _passive, 
        ref Ability[] _first, ref Ability[] _second)
    {
        _passive = m_passiveAbility;
        _first = m_firstAbilities;
        _second = m_secondAbilities;
    }

    public void SetAlreadyMoved(bool _moved) { m_alreadyMoved = _moved; }
    public void SetAlreadyAttacked(bool _attack) { m_alreadyAttacked = _attack; }

    public void SetAttackNodes(List<Node> _nodes) { m_attackNodes = _nodes; }
    public void SetLockedAbility(Ability _ability) { m_lockedAbility = _ability; }
    #endregion

    #region setters

    public void SetHP(int value) { m_HP = Mathf.Clamp(value, 0, m_maxHP); }
    public void SetIsFocused(bool _isFocused) { m_isFocused = _isFocused; }

    #endregion

    public void SetDuplicateMeshVisibilty(bool _isOff) { m_duplicateMesh.SetActive(_isOff); }
    
    // Start is called before the first frame update
    void Start()
    {
        m_HP = m_maxHP;
        m_attackNodes = new List<Node>();
    }

    public void MoveTo(Node _moveToNode)
    {
        GetCurrentNode().SetUnitOnTop(null);
        _moveToNode.SetUnitOnTop(this);

        m_coordinates = _moveToNode.GetCoordinates();
        transform.position = _moveToNode.GetNodePosition();
    }

    public void ShowAttackTiles()
    {
        foreach(Node node in m_attackNodes)
        {
            node.transform.GetComponent<MeshRenderer>().material.color = Color.red;
            node.m_selected = true;
        }
    }

    public void UnShowAttackTiles()
    {
        foreach (Node node in m_attackNodes)
        {
            node.transform.GetComponent<MeshRenderer>().material.color = Color.white;
            node.m_selected = false;
        }
    }

    public void MoveDuplicateTo(Node _moveToNode)
    {
        m_duplicateMesh.transform.position = _moveToNode.GetNodePosition();
    }

    public virtual void OnSelect()
    {
        m_onSelect.Invoke();

        if (GameManager.GetInstance().GetPlayerController().GetCurrentMode() == PlayerMode.MOVE &&
			!m_afterClear)
        { 
            
            m_availableNodes = Dijkstra.Instance.m_validMoves;

            foreach (Node node in m_availableNodes)
            {
                if (node.GetUnitOnTop())
                    continue;

                node.transform.GetComponent<MeshRenderer>().material.color = Color.red;
                node.m_selected = true;
            }           
        }

        if (GameManager.GetInstance().GetPlayerController().GetCurrentMode() == PlayerMode.ATTACK)
        {
            Node node = GetCurrentNode();
            node.OnSelect();
        }

        FindObjectOfType<Camera>().GetComponent<CameraMove>().MoveTo(transform.position, 1.0f);
    }

    public virtual void OnDeselect()
    {
        m_onDeselect.Invoke();

        Grid.GetNodeFromCoords(m_coordinates).OnDeselect();
    }

    public virtual void OnHover()
    {
        m_onHover.Invoke();
    }
    
    public virtual void OnUnhover()
    {
        m_onUnhover.Invoke();
    }

    public void TakeDamage(int damage)
    {
        m_HP -= damage;
        if (m_HP <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
		gameObject.SetActive(false);
		Node node = Grid.GetNodeFromCoords(m_coordinates);
		node.SetUnitOnTop(null);
		OnDie.Invoke();
    }
}
