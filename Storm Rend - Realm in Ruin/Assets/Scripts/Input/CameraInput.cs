using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraInput : MonoBehaviour
{
    private Camera m_camera;
    private CameraMove m_cameraMove;
    private CameraZoom m_cameraZoom;
    
    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        m_camera = GetComponent<Camera>();
        m_cameraMove = GetComponent<CameraMove>();
        m_cameraZoom = GetComponent<CameraZoom>();
    }

    /// <summary>
    /// 
    /// </summary>
    void Update()
    {
        if (m_cameraZoom != null)
            m_cameraZoom.StartZoom(Input.mouseScrollDelta.y * 5, 5);
    }
}
