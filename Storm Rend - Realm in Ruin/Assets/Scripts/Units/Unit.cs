using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Unit : MonoBehaviour, ISelectable, IHoverable
{
    [SerializeField] private MeshRenderer m_meshRenderer = null;
    [SerializeField] private GameObject m_duplicateMesh = null;

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

    #region getters

    public List<Node> GetAvailableNodes() { return m_availableNodes; }
    public Node GetCurrentNode() { return Grid.GetNodeFromCoords(m_coordinates); }
    public int GetMaxHP() { return m_maxHP; }
    public int GetHP() { return m_HP; }
    public int GetMove() { return m_maxMOV; }

    #endregion

    #region setters

    public void SetHP(int value) { m_HP = Mathf.Clamp(value, 0, m_maxHP); }

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
        PlayerController.SetCurrentPlayer(null);
    }

    public void MoveDuplicateTo(Node _moveToNode)
    {
        m_duplicateMesh.transform.position = _moveToNode.GetNodePosition();
    }

    public virtual void OnSelect()
    {
        m_onSelect.Invoke();

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
