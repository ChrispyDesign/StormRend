using System.Collections.Generic;
using UnityEngine;

namespace StormRend.CameraSystem
{
    /// <summary>
    /// camera zoom class, responsible for zooming in and out the camera
    /// </summary>
    [RequireComponent(typeof(CameraInput))]
    public class CameraZoom : MonoBehaviour
    {
        [SerializeField] Transform root = null;
        [SerializeField] float zoomSpeed = 10;

        [Header("Anchors")]
        [Tooltip("The nearest possible location the camera can zoom in to")]
        [SerializeField] Transform near = null;
        [Tooltip("The furthest possible location the camera can zoom out to")]
        [SerializeField] Transform far = null;
        [Tooltip("The transform containing all of the camera zoom anchor points")]
        [SerializeField] Transform anchorPoints = null;
        [Tooltip("The amount of transforms/snapping points to generate between near and far")]
        [SerializeField] int nearFarSteps = 5;

        // step/snapping point helper variables
        private List<GameObject> m_anchors = new List<GameObject>();
        private int m_currentStep = 0;

        // the position that the camera will consistently attempt to zoom to
        private Vector3 m_desiredPosition;

        //Members
        CameraInput input;

        void Awake()
        {
            input = GetComponent<CameraInput>();
        }

        void Start()
        {
            //create a list of anchor/snapping points
            CreateAnchors();

            //And jump to closest anchor
            GameObject closestAnchor = GetClosestAnchor(transform.position);
            m_currentStep = m_anchors.IndexOf(closestAnchor);
            m_desiredPosition = m_anchors[m_currentStep].transform.position - root.position;
        }

    #region Core
        /// <summary>
        /// update position during update (i'm lazy and didn't want to do a coroutine lol)
        /// </summary>
        void Update()
        {
            PollInput();
            ZoomCamera();
        }
        void PollInput()
        {
            if (input.zoomAxis > 0) ZoomBy(-1);
            if (input.zoomAxis < 0) ZoomBy(1);
        }
        void ZoomCamera()
        {
            // get current position and speed
            Vector3 currentPosition = transform.position;
            float speed = zoomSpeed * Time.unscaledDeltaTime;

            // update position
            Vector3 desiredPosition = m_desiredPosition + root.position;
            transform.position = Vector3.Lerp(currentPosition, desiredPosition, speed);

            // update current step based off the closest anchor to the desired position
            GameObject closestAnchor = GetClosestAnchor(desiredPosition);
            m_currentStep = m_anchors.IndexOf(closestAnchor);
        }
    #endregion

        /// <summary>
        /// helper function which compiles a list of anchor points, equidistant between the camera's near and far
        /// points
        /// </summary>
        void CreateAnchors()
        {
            // add near to list
            m_anchors.Add(this.near.gameObject);

            // use a for loop to create a given amount of steps/anchor points between near and far points
            for (int i = 0; i < nearFarSteps; i++)
            {
                // get all percentage values between near and far, not including 0% (near) and 100% (far)
                float percentage = (i + 1) / ((float)nearFarSteps + 1);

                // create empty transform
                GameObject step = new GameObject("Step"+i);
                step.transform.SetParent(anchorPoints);

                // adjust transform values
                step.transform.position = Vector3.Lerp(near.position, far.position, percentage);
                step.transform.rotation = transform.rotation;

                // add step to list
                m_anchors.Add(step);
            }

            // add far to list
            m_anchors.Add(this.far.gameObject);
        }

        /// <summary>
        /// use this function to zoom by a step amount! Jumps to the next step/anchor point depending on the input
        /// </summary>
        /// <param name="step">the amount of anchor points to step over</param>
        public void ZoomBy(int step)
        {
            // increment the step/anchor point and clamp it between 0 and step/anchor count
            m_currentStep += step;
            m_currentStep = Mathf.Clamp(m_currentStep, 0, nearFarSteps + 1);

            // update desired position
            m_desiredPosition = m_anchors[m_currentStep].transform.position - root.position;
        }

        /// <summary>
        /// use this function to zoom to a specified position!
        /// </summary>
        /// <param name="position">the position to zoom to</param>
        public void ZoomTo(Vector3 position)
        {
            m_desiredPosition = transform.position - root.position;
        }

        /// <summary>
        /// helper function which finds the closest anchor point to the given source point
        /// </summary>
        /// <param name="source">the source point to find the closest anchor from</param>
        /// <returns>the transform/object of the closest anchor point from the source</returns>
        GameObject GetClosestAnchor(Vector3 source)
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
}