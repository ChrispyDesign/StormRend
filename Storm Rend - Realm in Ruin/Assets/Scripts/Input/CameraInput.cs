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
        {
            if (Input.mouseScrollDelta.y > 0)
                m_cameraZoom.StartZoom(-1, 5);
            else if (Input.mouseScrollDelta.y < 0)
                m_cameraZoom.StartZoom(1, 5);
        }
    }
}
