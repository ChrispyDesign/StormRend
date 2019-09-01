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
    [SerializeField] private float m_zoomSpeed = 10;

    [Header("Zoom Anchors")]
    [Tooltip("The nearest possible location the camera can zoom in to")]
    [SerializeField] private Transform m_near = null;
    [Tooltip("The furthest possible location the camera can zoom out to")]
    [SerializeField] private Transform m_far = null;
    [Tooltip("The transform containing all of the camera zoom anchor points")]
    [SerializeField] private Transform m_anchorPoints;
    [Tooltip("The amount of transforms/snapping points to generate between near and far")]
    [SerializeField] private int m_nearFarSteps = 5;

    // step/snapping point helper variables
    private List<GameObject> m_anchors = new List<GameObject>();
    private int m_currentStep;

    // the position that the camera will consistently attempt to zoom to
    private Vector3 m_desiredPosition;

    /// <summary>
    /// create a list of anchor/snapping points at startup, and jump to the closest anchor
    /// </summary>
    void Start()
    {
        // create anchors/step positions
        CreateAnchors();

        // jump to closest anchor
        GameObject closestAnchor = GetClosestAnchor(transform.position);
        m_currentStep = m_anchors.IndexOf(closestAnchor);
        m_desiredPosition = m_anchors[m_currentStep].transform.position - m_rootTransform.position;
    }

    /// <summary>
    /// update position during update (i'm lazy and didn't want to do a coroutine lol)
    /// </summary>
    private void Update()
    {
        // get current position and speed
        Vector3 currentPosition = transform.position;
        float speed = m_zoomSpeed * Time.unscaledDeltaTime;

        // update position
        Vector3 desiredPosition = m_desiredPosition + m_rootTransform.position;
        transform.position = Vector3.Lerp(currentPosition, desiredPosition, speed);

        // update current step based off the closest anchor to the desired position
        GameObject closestAnchor = GetClosestAnchor(desiredPosition);
        m_currentStep = m_anchors.IndexOf(closestAnchor);
    }

    /// <summary>
    /// helper function which compiles a list of anchor points, equidistant between the camera's near and far
    /// points
    /// </summary>
    private void CreateAnchors()
    {
        // get near/far positions
        Vector3 near = m_near.position;
        Vector3 far = m_far.position;

        // add near to list
        m_anchors.Add(m_near.gameObject);

        // use a for loop to create a given amount of steps/anchor points between near and far points
        for (int i = 0; i < m_nearFarSteps; i++)
        {
            // get all percentage values between near and far, not including 0% (near) and 100% (far)
            float percentage = (i + 1) / ((float)m_nearFarSteps + 1);

            // create empty transform
            GameObject step = new GameObject();
            step.transform.SetParent(m_anchorPoints);

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
    /// use this function to zoom by a step amount! Jumps to the next step/anchor point depending on the input
    /// </summary>
    /// <param name="step">the amount of anchor points to step over</param>
    public void ZoomBy(int step)
    {
        // increment the step/anchor point and clamp it between 0 and step/anchor count
        m_currentStep += step;
        m_currentStep = Mathf.Clamp(m_currentStep, 0, m_nearFarSteps + 1);

        // update desired position
        m_desiredPosition = m_anchors[m_currentStep].transform.position - m_rootTransform.position;
    }

    /// <summary>
    /// use this function to zoom to a specified position!
    /// </summary>
    /// <param name="position">the position to zoom to</param>
    public void ZoomTo(Vector3 position)
    {
        m_desiredPosition = transform.position - m_rootTransform.position;
    }

    /// <summary>
    /// helper function which finds the closest anchor point to the given source point
    /// </summary>
    /// <param name="source">the source point to find the closest anchor from</param>
    /// <returns>the transform/object of the closest anchor point from the source</returns>
    private GameObject GetClosestAnchor(Vector3 source)
    {
        // output
        GameObject closestAnchor = null;

        // minimum distance is infinity right now
        float minDistance = Mathf.Infinity;

        // linearly search all anchor points
        for (int i = 0; i < m_anchors.Count; i++)
        {
            GameObject anchor = m_anchors[i];

            // get distance from source to destination
            float distance = Vector3.Distance(source, anchor.transform.position);

            // perform distance comparison
            if (distance < minDistance)
            {
                // new minimum distance/closest anchor
                minDistance = distance;
                closestAnchor = anchor;
            }
        }

        return closestAnchor;
    }
}