using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace StormRend.CameraSystem
{
    [RequireComponent(typeof(CameraInput))]
    public class xCameraZoom : MonoBehaviour
    {
        [SerializeField] Transform root = null;
        [SerializeField] float zoomSpeed = 10;

        [Header("Anchors")]
        [Tooltip("The nearest zoom point the camera can zoom in to")]
        [SerializeField] Transform near = null;
        [Tooltip("The furthest zoom point the camera can zoom out to")]
        [SerializeField] Transform far = null;
        [Tooltip("The transform containing all of the camera zoom anchor points")]
        [SerializeField] Transform anchorPoints = null;
        [Tooltip("The amount of intermediate zoom points to generate")]
        [SerializeField] int zoomPoints = 5;


		//Properties
        public int currentZoomPoint
		{
			get => _currentZoomPoint;
			private set => _currentZoomPoint = Mathf.Clamp(value, 0, zoomPoints + 1);
		} 

        // the position that the camera will consistently attempt to zoom to

		//Members
        private List<GameObject> intermediateZoomPoints = new List<GameObject>();
        Vector3 desiredZoomPosition;
		float desiredFocusDistance = 1f;
		int _currentZoomPoint = 0;
		CameraInput input;
		// PostProcessVolume ppv = null;
		[SerializeField] PostProcessProfile ppp = null;
		DepthOfField dof = null;
		[SerializeField] float zoomLerp = 0.2f;
		[SerializeField] float focusLerp = 0.05f;

	#region Init
		void Awake()
        {
            input = GetComponent<CameraInput>();
			if (!ppp.TryGetSettings<DepthOfField>(out dof))
				Debug.Log("No Depth of Field effect found!");
        }

        void Start()
        {
            //create a list of anchor/snapping points
            CreateAnchors();

            //And jump to closest anchor
            GameObject closestAnchor = GetClosestAnchor(transform.localPosition);
            currentZoomPoint = intermediateZoomPoints.IndexOf(closestAnchor);
            desiredZoomPosition = intermediateZoomPoints[currentZoomPoint].transform.position - root.position;
        }

		/// <summary>
		/// helper function which compiles a list of anchor points, equidistant between the camera's near and far
		/// points
		/// </summary>
		void CreateAnchors()
		{
			// add near to list
			intermediateZoomPoints.Add(this.near.gameObject);

			// use a for loop to create a given amount of steps/anchor points between near and far points
			for (int i = 0; i < zoomPoints; i++)
			{
				// get all percentage values between near and far, not including 0% (near) and 100% (far)
				float percentage = (i + 1) / ((float)zoomPoints + 1);

				// create empty transform
				GameObject step = new GameObject("Step" + i);
				step.transform.SetParent(anchorPoints);

				// adjust transform values
				step.transform.position = Vector3.Lerp(near.position, far.position, percentage);
				step.transform.rotation = transform.rotation;

				// add step to list
				intermediateZoomPoints.Add(step);
			}

			// add far to list
			intermediateZoomPoints.Add(this.far.gameObject);
		}
	#endregion

	#region Core
		/// <summary>
		/// update position during update (i'm lazy and didn't want to do a coroutine lol)
		/// </summary>
		void Update()
        {
            PollInput();
        }

		void LateUpdate()
		{
			transform.localPosition = Vector3.Lerp(transform.localPosition, desiredZoomPosition, zoomLerp);
			dof.focusDistance.value = Mathf.Lerp(dof.focusDistance.value, desiredFocusDistance, focusLerp);
		}

        void PollInput()
        {
            if (input.zoomAxis > 0) ZoomStep(-1);
            if (input.zoomAxis < 0) ZoomStep(1);
        }

        // void ZoomCamera()
        // {
        //     // get current position and speed
        //     Vector3 currentPosition = transform.position;
        //     float speed = zoomSpeed * Time.unscaledDeltaTime;

        //     // update position
        //     Vector3 localDesiredPosition = desiredPosition + root.position;
        //     transform.position = Vector3.Lerp(currentPosition, localDesiredPosition, speed);

        //     // update current step based off the closest anchor to the desired position
        //     // GameObject closestAnchor = GetClosestAnchor(localDesiredPosition);
        //     // currentZoomPoint = intermediateZoomPoints.IndexOf(closestAnchor);
        // }
    #endregion

        public void ZoomStep(int step)
        {
            currentZoomPoint += step;

			//Set position to lerp to
            desiredZoomPosition = intermediateZoomPoints[currentZoomPoint].transform.position - root.position;
				Debug.Log("Desired zoom pos: " + desiredZoomPosition);

			//Set focal distances to lerp to
			desiredFocusDistance = Vector3.Distance(desiredZoomPosition, root.position);
				Debug.Log("Desired focus dist: " + desiredFocusDistance);
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
            for (int i = 0; i < intermediateZoomPoints.Count; i++)
            {
                GameObject anchor = intermediateZoomPoints[i];

                // get distance from source to destination
                float distance = Vector3.Distance(source, anchor.transform.position);	//THIS WAS CALLED EVERY FRAME!!! FUCKING STUPID!

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