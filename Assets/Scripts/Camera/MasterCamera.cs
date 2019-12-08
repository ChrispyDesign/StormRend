/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

using pokoro.Patterns.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace StormRend.CameraSystem
{
	/// <summary>
	/// This is used as a tag to find locate the main working camera.
	/// If you use cinemachine you'd only need one master camera
	/// </summary>
	[RequireComponent(typeof(CameraInput), typeof(CameraMover), typeof(CameraZoomer))]
	public class MasterCamera : Singleton<MasterCamera>
	{
		//Properties
		public new Camera camera { get; private set; }
		public CameraInput cameraInput { get; private set; }
		public CameraMover cameraMover { get; private set; }
		public CameraZoomer cameraZoom { get; private set; }
		public PhysicsRaycaster physicsRaycaster { get; private set; }

		void Reset()
		{
			camera = GetComponent<Camera>();
			if (camera) return;
			camera = GetComponentInChildren<Camera>();	//Find below
			if (camera) return;
			camera = GetComponentInParent<Camera>();		//Find above
			Debug.Assert(camera, "No camera found on this object!");
		}

		void Awake()
		{
			camera = GetComponentInChildren<Camera>();
			cameraInput = GetComponent<CameraInput>();
			cameraMover = GetComponent<CameraMover>();
			cameraZoom = GetComponent<CameraZoomer>();
			physicsRaycaster = GetComponentInChildren<PhysicsRaycaster>();
		}
	}
}