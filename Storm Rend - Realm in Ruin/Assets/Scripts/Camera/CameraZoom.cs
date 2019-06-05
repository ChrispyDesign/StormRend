using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraZoom : MonoBehaviour
{
    private Camera m_camera;
    
    //[SerializeField] private float m_minZoom = 5;
    //[SerializeField] private float m_maxZoom = 20;
    private Vector3 m_desiredPosition;

    /// <summary>
    /// cache camera component
    /// </summary>
    void Start()
    {
        m_camera = GetComponent<Camera>();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="delta"></param>
    /// <param name="lerpTime"></param>
    public void StartZoom(float delta, float zoomSpeed)
    {
        Vector3 currentPosition = m_camera.transform.position;
        if (delta != 0)
            m_desiredPosition = currentPosition + m_camera.transform.forward * delta;

        m_camera.transform.position = Vector3.Lerp(currentPosition, m_desiredPosition, zoomSpeed * Time.deltaTime);
    }
}
