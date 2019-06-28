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
        if (Physics.Raycast(ray, out raycastHit))
        {
            GameObject hitObject = raycastHit.collider.gameObject;

            if (m_hoveredObject != hitObject)
                Hover(hitObject); // new hovered object

            if (Input.GetMouseButtonDown(0) && m_selectedObject != hitObject)
                Select(hitObject); // select (left click)
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="hitObject"></param>
    private void Hover(GameObject hitObject)
    {
        if (m_unhoveredObject != m_hoveredObject)
        {
            m_unhoveredObject = m_hoveredObject; // store unhovered object

            IHoverable unhoveredClickable = m_unhoveredObject.GetComponent<IHoverable>();
            if (unhoveredClickable != null)
                unhoveredClickable.OnUnhover();
        }

        m_hoveredObject = hitObject; // store hovered object

        IHoverable hoveredClickable = m_hoveredObject.GetComponent<IHoverable>();
        if (hoveredClickable != null)
            hoveredClickable.OnHover();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="hitObject"></param>
    private void Select(GameObject hitObject)
    {
        if (m_deselectedObject != m_selectedObject)
        {
            m_deselectedObject = m_selectedObject; // store deselected object

            ISelectable deselectedClickable = m_deselectedObject.GetComponent<ISelectable>();
            if (deselectedClickable != null)
                deselectedClickable.OnDeselect();
        }

        m_selectedObject = hitObject; // store hit object

        ISelectable selectedClickable = m_selectedObject.GetComponent<ISelectable>();
        if (selectedClickable != null)
            selectedClickable.OnSelect();
    }
}
