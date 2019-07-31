using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Unit : MonoBehaviour, ISelectable, IHoverable
{
    [SerializeField] private MeshRenderer m_meshRenderer = null;
    [SerializeField] private GameObject m_duplicateMesh = null;
    [SerializeField] private Ability m_passiveAbility;
    [SerializeField] private Ability[] m_firstAbilities;
    [SerializeField] private Ability[] m_secondAbilities;

    [Header("Unit Stats")]
    [SerializeField] private int m_maxHP = 4;
    [SerializeField] private int m_maxMOV = 4;
    private int m_HP;
    
    [Space]
    [Header("Unit Interaction")]
    [SerializeField] private UnityEvent m_onSelect;
    [SerializeField] private UnityEvent m_onDeselect;
    [SerializeField] private UnityEvent m_onHover;
    [SerializeField] private UnityEvent m_onUnhover;

    public Vector2Int m_coordinates;
    private List<Node> m_availableNodes;
    private List<Node> m_attackNodes;
    protected bool m_isFocused;
    protected Ability m_lockedAbility;
   
    #region getters

    public List<Node> GetAvailableNodes() { return m_availableNodes; }
    public Ability GetLockedAbility() { return m_lockedAbility; }
    public List<Node> GetAttackNodes() { return m_attackNodes; }
    public Node GetCurrentNode() { return Grid.GetNodeFromCoords(m_coordinates); }
    public int GetMaxHP() { return m_maxHP; }
    public int GetHP() { return m_HP; }
    public int GetMove() { return m_maxMOV; }
    public bool GetIsFocused() { return m_isFocused; }

    public void GetAbilities( ref Ability _passive, 
        ref Ability[] _first, ref Ability[] _second)
    {
        _passive = m_passiveAbility;
        _first = m_firstAbilities;
        _second = m_secondAbilities;
    }
    
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

        if (PlayerController.GetCurrentMode() == PlayerMode.MOVE)
        { 
            
            m_availableNodes = Dijkstra.Instance.m_validMoves;

            foreach (Node node in m_availableNodes)
            {
                if (node.GetUnitOnTop())
                    continue;

                node.transform.GetComponent<MeshRenderer>().material.color = Color.red;
                node.m_selected = true;
            }
        
            Color materialColour = m_meshRenderer.material.color;
            m_meshRenderer.material.color = new Color(materialColour.r, materialColour.g, materialColour.b, 0.5f);
        }

        if (PlayerController.GetCurrentMode() == PlayerMode.ATTACK)
        {
            Unit player = PlayerController.GetCurrentPlayer();
            Node node = GetCurrentNode();
            if (node.m_selected)
            {
                Ability ability = player.GetLockedAbility();
                foreach(Effect effect in ability.GetEffects())
                {
                    effect.PerformEffect(node);
                }
            }
            PlayerController.SetCurrentMode(PlayerMode.IDLE);
        }

        FindObjectOfType<Camera>().GetComponent<CameraMove>().MoveTo(transform.position, 1.0f);
    }

    public virtual void OnDeselect()
    {
        m_onDeselect.Invoke();

        Grid.GetNodeFromCoords(m_coordinates).OnDeselect();

        Color materialColour = m_meshRenderer.material.color;
        m_meshRenderer.material.color = new Color(materialColour.r, materialColour.g, materialColour.b, 1);
    }

    public virtual void OnHover()
    {
        m_onHover.Invoke();
    }
    
    public virtual void OnUnhover()
    {
        m_onUnhover.Invoke();
    }
}
