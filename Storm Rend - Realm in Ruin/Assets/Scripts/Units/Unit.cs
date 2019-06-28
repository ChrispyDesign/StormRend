using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour, ISelectable, IHoverable
{
    [SerializeField] private MeshRenderer m_meshRenderer;

    // Start is called before the first frame update
    void Start()
    {
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
