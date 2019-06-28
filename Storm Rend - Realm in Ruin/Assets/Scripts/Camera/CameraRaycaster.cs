using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraRaycaster : MonoBehaviour
{
    private Camera m_camera;

    private static GameObject m_hoveredObject;
    private static GameObject m_unhoveredObject;
    private static GameObject m_selectedObject;
    private static GameObject m_deselectedObject;

    #region getters

    public static GameObject GetHoveredObject() { return m_hoveredObject; }
    public static GameObject GetUnhoveredObject() { return m_unhoveredObject; }
    public static GameObject GetSelectedObject() { return m_selectedObject; }
    public static GameObject GetDeselectedObject() { return m_deselectedObject; }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        m_camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        PerformRaycast();
    }

    /// <summary>
    /// 
    /// </summary>
    private void PerformRaycast()
    {
        Ray ray = m_camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit;

        // perform raycast
        Physics.Raycast(ray, out raycastHit);
        GameObject hitObject = null;

        if (raycastHit.collider != null)
            hitObject = raycastHit.collider.gameObject;

        if (m_hoveredObject != hitObject)
            Hover(hitObject); // new hovered object

        if (Input.GetMouseButtonDown(0))
            Select(hitObject); // select (left click)
        
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="hitObject"></param>
    private void Hover(GameObject hitObject)
    {
        if (m_unhoveredObject != m_hoveredObject)
        {
            // store unhovered object
            m_unhoveredObject = m_hoveredObject; 

            // unhovered object can be null, so perform safety check
            if (m_unhoveredObject)
            {
                IHoverable unhoveredClickable = m_unhoveredObject.GetComponent<IHoverable>();
                if (unhoveredClickable != null)
                    unhoveredClickable.OnUnhover(); // call unhover on unhovered object
            }
        }

        // store hovered object
        m_hoveredObject = hitObject;

        // hovered object can be null, so perform safety check
        if (m_hoveredObject)
        {
            IHoverable hoveredClickable = m_hoveredObject.GetComponent<IHoverable>();
            if (hoveredClickable != null)
                hoveredClickable.OnHover(); // call hover on hovered object
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="hitObject"></param>
    private void Select(GameObject hitObject)
    {
        if (m_deselectedObject != m_selectedObject)
        {
            // store deselected object
            m_deselectedObject = m_selectedObject;

            // deselected object can be null, so perform safety check
            if (m_deselectedObject)
            {
                ISelectable deselectedClickable = m_deselectedObject.GetComponent<ISelectable>();
                if (deselectedClickable != null)
                    deselectedClickable.OnDeselect(); // call deselect on deselected object
            }
        }

        // store hit object
        m_selectedObject = hitObject;

        // selected object can be null, so perform safety check
        if (m_selectedObject)
        {
            ISelectable selectedClickable = m_selectedObject.GetComponent<ISelectable>();
            if (selectedClickable != null)
                selectedClickable.OnSelect(); // call select on selected object
        }
    }
}
