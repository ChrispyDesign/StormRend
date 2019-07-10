using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// camera zoom class, responsible for zooming in and out the camera
/// </summary>
[RequireComponent(typeof(Camera))]
public class CameraZoom : MonoBehaviour
{
    [Header("Root Transform")]
    [SerializeField] private Transform m_rootTransform = null;

    [Header("Zoom Speed")]
    [SerializeField] private float m_zoomSpeed = 1;

    [Header("Zoom Anchors")]
    [Tooltip("The nearest possible location the camera can zoom in to")]
    [SerializeField] private Transform m_near = null;
    [Tooltip("The furthest possible location the camera can zoom out to")]
    [SerializeField] private Transform m_far = null;
    [Tooltip("The amount of transforms/snapping points to generate between near and far")]
    [SerializeField] private int m_nearFarSteps = 5;

    // step/snapping point helper variables
    private List<GameObject> m_anchors = new List<GameObject>();
    private int m_currentStep;

    private Camera m_camera;
    private Vector3 m_desiredPosition;
    
    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        // cache camera reference
        m_camera = GetComponent<Camera>();
        m_desiredPosition = transform.position;

        // create anchors/step positions
        CreateAnchors();

        // jump to closest anchor on startup
        GameObject closestAnchor = GetClosestAnchor(m_camera.transform.position);
        m_currentStep = m_anchors.IndexOf(closestAnchor);
        m_desiredPosition = m_anchors[m_currentStep].transform.position;
    }

    /// <summary>
    /// 
    /// </summary>
    private void Update()
    {
        // get current position and speed
        Vector3 currentPosition = m_camera.transform.position;
        float speed = m_zoomSpeed * Time.deltaTime;

        // update position
        Vector3 desiredPosition = m_desiredPosition + m_rootTransform.position;
        m_camera.transform.position = Vector3.Lerp(currentPosition, desiredPosition, speed);

        // update current step based off the closest anchor to the desired position
        GameObject closestAnchor = GetClosestAnchor(desiredPosition);
        m_currentStep = m_anchors.IndexOf(closestAnchor);
    }

    /// <summary>
    /// 
    /// </summary>
    private void CreateAnchors()
    {
        // add near to list
        m_anchors.Add(m_near.gameObject);

        for (int i = 1; i <= m_nearFarSteps; i++)
        {
            float percentage = i / ((float)m_nearFarSteps + 1);
            Vector3 near = m_near.position;
            Vector3 far = m_far.position;

            // create empty transform
            GameObject step = new GameObject();
            step.transform.SetParent(m_camera.transform.parent);

            // adjust transform values
            step.transform.position = Vector3.Lerp(near, far, percentage);
            step.transform.rotation = transform.rotation;

            // add step to list
            m_anchors.Add(step);
        }

        // add far to list
        m_anchors.Add(m_far.gameObject);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="step"></param>
    /// <param name="lerpTime"></param>
    public void StartZoom(int step, float zoomSpeed)
    {
        m_currentStep += step;
        m_currentStep = Mathf.Clamp(m_currentStep, 0, m_nearFarSteps + 1);
        
        m_desiredPosition = m_anchors[m_currentStep].transform.position - m_rootTransform.position;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="transform"></param>
    public void ZoomTo(Transform transform)
    {
        m_desiredPosition = transform.position;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private GameObject GetClosestAnchor(Vector3 source)
    {
        GameObject closestAnchor = null;
        float minDistance = Mathf.Infinity;

        for (int i = 0; i < m_anchors.Count; i++)
        {
            GameObject anchor = m_anchors[i];

            float distance = Vector3.Distance(source, anchor.transform.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                closestAnchor = anchor;
            }
        }

        return closestAnchor;
    }
}
