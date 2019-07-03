using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour, ISelectable, IHoverable
{
    [SerializeField] private MeshRenderer m_meshRenderer = null;

    #region unit stats
    
    [SerializeField] private int m_maxHP = 4;
    [SerializeField] private int m_maxMOV = 4;
    private int m_HP;

    public int GetHP() { return m_HP; }
    public void SetHP(int value) { m_HP = Mathf.Clamp(value, 0, m_maxHP); }

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
        m_meshRenderer.material.color = Color.blue;
    }

    public virtual void OnDeselect()
    {
        m_meshRenderer.material.color = Color.white;
    }

    public virtual void OnHover()
    {
        m_meshRenderer.material.color = Color.green;
    }
    
    public virtual void OnUnhover()
    {
        m_meshRenderer.material.color = Color.white;
    }
}
