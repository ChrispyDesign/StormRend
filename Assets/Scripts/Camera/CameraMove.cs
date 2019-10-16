using System;
using System.Collections;
using UnityEngine;

namespace StormRend.CameraSystem
{
    /// <summary>
    /// camera movement class, responsible for the translation of the camera
    /// </summary>
    [RequireComponent(typeof(CameraInput))]
    public class CameraMove : MonoBehaviour
    {
        [SerializeField] Transform rootTransform = null;
        [SerializeField] float moveSpeed = 10;

        [SerializeField] BoxCollider cameraBounds = null;

        //Members
        CameraInput input;

    #region Core
        void Awake()
        {
            input = GetComponent<CameraInput>();
        }
        void Update()
        {
            var moveAxis = new Vector3(input.xAxis, 0, input.yAxis);
            MoveBy(moveAxis);
        }
    #endregion

        /// <summary>
        /// use this to move the camera by an incremental amount!
        /// </summary>
        /// <param name="axis">the value in each axis to move</param>
        public void MoveBy(Vector3 axis)
        {
            // stop movement if the MoveTo coroutine is already executing
            // if (moveTo != null && axis != Vector3.zero)
            StopCoroutine(LerpTo(Vector3.zero));

            float speed = moveSpeed * Time.unscaledDeltaTime;

            // determine the destination of the end of the movement
            Vector3 destination = rootTransform.position;
            destination += axis.z * rootTransform.forward * speed;
            destination += axis.y * rootTransform.up * speed;
            destination += axis.x * rootTransform.right * speed;

            // ensure camera stays within bounds
            destination = ClampDestination(destination);

            // perform movement
            rootTransform.position = destination;
        }

        /// <summary>
        /// use this to move the camera to a destination over an arbitrary amount of time!
        /// </summary>
        /// <param name="destination">the position to lerp/move to</param>
        /// <param name="time">the amount of time it takes to lerp to the destination</param>
        public void MoveTo(Vector3 destination, float time = 0.3f)
        {
            // stop movement if the MoveTo coroutine is already executing
            // if (moveTo != null)
            StopCoroutine(LerpTo(destination, time));

            // ensure camera stays within bounds
            destination = ClampDestination(destination);

            // start new MoveTo coroutine
            StartCoroutine(LerpTo(destination, time));
        }

        /// <summary>
        /// lerp/move coroutine which lerps the camera from it's current position, to a destination in an
        /// arbitrary amount of time
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
                rootTransform.position = Vector3.Lerp(rootTransform.position, destination, t);
                yield return null;
            }
        }

        /// <summary>
        /// magic function, spend hours on writing this one (no joke)
        /// </summary>
        /// <param name="destination">the destination to clamp</param>
        /// <returns>the clamped destination</returns>
        Vector3 ClampDestination(Vector3 destination)
        {
            return cameraBounds.ClosestPoint(destination);
        }
    }
}