using System;
using System.Collections;
using StormRend.Units;
using UnityEngine;

namespace StormRend.CameraSystem
{
    /// <summary>
    /// camera movement class, responsible for the translation of the camera
    /// </summary>
    [RequireComponent(typeof(CameraInput))]
    public class CameraMover : MonoBehaviour
    {
        //Inspector
        [SerializeField] Transform root = null;
        [SerializeField] float moveSpeed = 10;
        [SerializeField] BoxCollider cameraBounds = null;

        //Members
        CameraInput cin;

    #region Core
        void Awake()
        {
            cin = GetComponent<CameraInput>();
        }
        void Update()
        {
            var moveAxis = new Vector3(cin.xAxis, 0, cin.yAxis);
			//If there's input then override any current moving coroutines
			if (!moveAxis.Equals(Vector3.zero))
            	MoveBy(moveAxis);
        }
    #endregion

        /// <summary>
        /// Use this to move the camera by an incremental amount!
        /// </summary>
        /// <param name="axis">the value in each axis to move</param>
        public void MoveBy(Vector3 axis)
        {
			//Override any current lerps
			StopAllCoroutines();

            float speed = moveSpeed * Time.unscaledDeltaTime;

            // determine the destination of the end of the movement
            Vector3 destination = root.position;
            destination += axis.z * root.forward * speed;
            destination += axis.y * root.up * speed;
            destination += axis.x * root.right * speed;

            // ensure camera stays within bounds
            ClampDestination(ref destination);

            // perform movement
            root.position = destination;
        }

        public void MoveTo(Unit unit, float smoothTime = 0.3f)
        {
			//Override any current lerps
			StopAllCoroutines();

            MoveTo(unit.transform.position, smoothTime);
        }

        /// <summary>
        /// Use this to move the camera to a destination over an arbitrary amount of time!
        /// </summary>
        /// <param name="destination">the position to lerp/move to</param>
        /// <param name="smoothTime">the amount of time it takes to lerp to the destination</param>
        public void MoveTo(Vector3 destination, float smoothTime = 0.3f)
        {
			//Override any current lerps
			StopAllCoroutines();

			//Clamp within bounds
            ClampDestination(ref destination);

			//Start move
            StartCoroutine(LerpTo(destination, smoothTime));
        }

        /// <summary>
        /// Lerp/move coroutine which lerps the camera from it's current position, 
		/// to a destination in an arbitrary amount of time
        /// </summary>
        /// <param name="destination">the position to lerp/move to</param>
        /// <param name="time">the amount of time it takes to lerp to the destination</param>
        IEnumerator LerpTo(Vector3 destination, float time = 0.3f)
        {
            float timer = 0;

            while (timer < time)
            {
                // get lerp percentage & increment timer
                float t = timer / time;
                timer += Time.unscaledDeltaTime;

                // perform incremental movement
                root.position = Vector3.Lerp(root.position, destination, t);
                yield return null;
            }
        }

        /// <summary>
        /// Magic function, spend hours on writing this one (no joke)
        /// </summary>
        /// <param name="destination">the destination to clamp</param>
        /// <returns>the clamped destination</returns>
        void ClampDestination(ref Vector3 destination)
        {
            destination = cameraBounds.ClosestPoint(destination);
        }
    }
}