using System.Collections;
using UnityEngine;

/// <summary>
/// camera movement class, responsible for the translation of the camera
/// </summary>
[RequireComponent(typeof(Camera))]
public class CameraMove : MonoBehaviour
{
    [Header("Move Speed")]
    [SerializeField] private float m_moveSpeed = 1;

    [Header("Move Anchors")]
    [SerializeField] private Vector2 m_anchorExtents = new Vector2(1, 1);

    private Camera m_camera;

    /// <summary>
    /// cache camera component
    /// </summary>
    void Start()
    {
        m_camera = GetComponent<Camera>();
    }
}
