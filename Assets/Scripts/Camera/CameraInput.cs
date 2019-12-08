/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

using UnityEngine;

namespace StormRend.CameraSystem
{
	public class CameraInput : MonoBehaviour
	{
		public enum MouseButton : int
		{
			Left = 0,
			Right = 1,
			Middle = 2
		}

		//Inspector
		[SerializeField] string xAxisName = "Horizontal";
		[SerializeField] string yAxisName = "Vertical";

		[Header("Drag")]
		[Tooltip("The amount of mouse delta required to enter drag mode")]
		[SerializeField] float dragThreshold = 1f;
		[SerializeField] MouseButton dragMouseButton = MouseButton.Right;

		//Properties
		public float xAxis { get; private set; }
		public float yAxis { get; private set; }
		public float zoomAxis { get; private set; }
		public Vector2 mousePosition { get; private set; }
		public Vector3? drag { get; private set; }

		//Members
		Vector3 lastPos;

		void Update()
		{
			PollMoveInput(); // can perform translation
			PollZoomInput(); // can perform zooming
			PollMousePosition();    //Edge panning
			PollMouseDrag();        //Camera drag
		}

		void PollMoveInput()
		{
			xAxis = Input.GetAxisRaw(xAxisName);
			yAxis = Input.GetAxisRaw(yAxisName);
		}

		void PollZoomInput()
		{
			zoomAxis = Input.mouseScrollDelta.y;
		}

		void PollMousePosition()
		{
			mousePosition = Input.mousePosition;
		}

		/// <summary>
		/// NOTE! CameraInput execution order MUST BE AFTER UserInputHandler for camera drag to work correctly!!!
		/// </summary>
		void PollMouseDrag()
		{
			//MouseClick: Record drag origin point on first clicking
			if (Input.GetMouseButtonDown((int)dragMouseButton))
			{
				// print("Begin dragging");
				lastPos = Input.mousePosition;
			}
			//MouseHold: Update drag amount
			else if (Input.GetMouseButton((int)dragMouseButton))
			{
				// print("Dragging");
				var difference = Input.mousePosition - lastPos;
				// print("difference: " + difference);
				if (difference.sqrMagnitude > dragThreshold)
				{
					// print("dragging");
					drag = Input.mousePosition - lastPos;
					lastPos = Input.mousePosition;
				}
				else
				{
					// print("not draggin");
					drag = null;
				}
			}
			//MouseReleased: set drag to null
			else if (Input.GetMouseButtonUp((int)dragMouseButton))
			{
				// print("Stop dragging");
				drag = null;
			}
		}
	}
}