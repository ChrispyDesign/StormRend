using UnityEngine;

/// <summary>
/// camera input class responsible for moving and zooming the camera on player input
/// </summary>
[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(CameraMove))]
[RequireComponent(typeof(CameraZoom))]
public class CameraInput : MonoBehaviour
{
    // camera components
    private Camera m_camera;
    private CameraMove m_cameraMove;
    private CameraZoom m_cameraZoom;

    // utility variables
    private static bool m_canMoveCamera = true;
    private static bool m_canZoomCamera = true;
    
    /// <summary>
    /// cache references to camera and camera move/zoom scripts
    /// </summary>
    void Start()
    {
        // cache relevant components
        m_camera = GetComponent<Camera>();
        m_cameraMove = GetComponent<CameraMove>();
        m_cameraZoom = GetComponent<CameraZoom>();

        // error handling
        Debug.Assert(m_cameraMove, "CameraMove script not found on camera!");
        Debug.Assert(m_cameraZoom, "CameraZoom script not found on camera!");
    }

    /// <summary>
    /// check for player input every frame
    /// </summary>
    void Update()
    {
        if (m_canMoveCamera)
            Move(); // can perform translation

        if (m_canZoomCamera)
            Zoom(); // can perform zooming
    }

    /// <summary>
    /// movement function which uses the horizontal and vertical axes to translate the camera (subject to change)
    /// </summary>
    private void Move()
    {
        float xAxis = Input.GetAxisRaw("Horizontal");
        float zAxis = Input.GetAxisRaw("Vertical");

        // if input is received
        if (xAxis != 0 || zAxis != 0)
            m_cameraMove.MoveBy(new Vector3(xAxis, 0, zAxis)); // perform movement
    }

    /// <summary>
    /// zoom function which uses the mouse scroll wheel to zoom the camera in and out (subject to change)
    /// </summary>
    private void Zoom()
    {
        // dirty way of zooming, should change
        if (Input.mouseScrollDelta.y > 0)
            m_cameraZoom.ZoomBy(-1);
        else if (Input.mouseScrollDelta.y < 0)
            m_cameraZoom.ZoomBy(1);
    }

    #region utility functions

    /// <summary>
    /// use this to enable/disable camera zoom and translation!
    /// </summary>
    /// <param name="value">true to freeze camera, false to unfreeze it</param>
    public static void FreezeCamera(bool value)
    {
        m_canMoveCamera = !value;
        m_canZoomCamera = !value;
    }

    /// <summary>
    /// use this to enable/disable camera translation!
    /// </summary>
    /// <param name="value">true to freeze camera translation, false to unfreeze it</param>
    public static void FreezeCameraTranslation(bool value)
    {
        m_canMoveCamera = !value;
    }

    /// <summary>
    /// use this to enable/disable camera zoom!
    /// </summary>
    /// <param name="value">true to freeze camera zoom, false to unfreeze it</param>
    public static void FreezeCameraZoom(bool value)
    {
        m_canZoomCamera = !value;
    }

    #endregion
}