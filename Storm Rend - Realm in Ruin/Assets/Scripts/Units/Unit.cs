using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour, ISelectable, IHoverable
{
    [SerializeField] private MeshRenderer m_meshRenderer = null;
    [SerializeField] private GameObject m_duplicateMesh = null;

    public Vector2Int m_coordinates;
    private List<Node> m_availableNodes;

    #region unit stats

    [SerializeField] private int m_maxHP = 4;
    [SerializeField] private int m_maxMOV = 4;
    private int m_HP;

    public int GetHP() { return m_HP; }
    public void SetHP(int value) { m_HP = Mathf.Clamp(value, 0, m_maxHP); }

    public int GetMove() { return m_maxMOV; }

    #endregion

    #region getters

    public List<Node> GetAvailableNodes() { return m_availableNodes; }

    #endregion

    public void SetDuplicateMeshVisibilty(bool _isOff) { m_duplicateMesh.SetActive(_isOff); }

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
        PlayerController.SetCurrentPlayer(null);
    }

    public void MoveDuplicateTo(Node _moveToNode)
    {
        m_duplicateMesh.transform.position = _moveToNode.GetNodePosition();
    }

    public virtual void OnSelect()
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

        FindObjectOfType<Camera>().GetComponent<CameraMove>().MoveTo(transform.position, 1.0f);
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
