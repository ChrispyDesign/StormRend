using pokoro.Patterns.Generic;
using UnityEngine;

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
		public new Camera camera 
		{
			get 
			{
				return _cam;
			}
			private set => _cam = value;
		}
		public CameraInput cameraInput => _ci;
		public CameraMover cameraMover => _cm;
		public CameraZoomer cameraZoom => _cz;

		//Members
		Camera _cam = null;
		CameraInput _ci = null;
		CameraMover _cm = null;
		CameraZoomer _cz = null;

		void Reset()
		{
			_cam = GetComponent<Camera>();
			if (_cam) return;
			_cam = GetComponentInChildren<Camera>();	//Find below
			if (_cam) return;
			_cam = GetComponentInParent<Camera>();		//Find above
			Debug.Assert(_cam, "No camera found on this object!");
		}

		void Awake()
		{
			_cam = GetComponentInChildren<Camera>();
			_ci = GetComponent<CameraInput>();
			_cm = GetComponent<CameraMover>();
			_cz = GetComponent<CameraZoomer>();
		}
	}
}