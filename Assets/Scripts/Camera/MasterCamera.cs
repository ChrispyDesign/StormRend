using pokoro.Patterns.Generic;
using UnityEngine;

namespace StormRend.CameraSystem
{
	/// <summary>
	/// This is used as a tag to find locate the main working camera.
	/// If you use cinemachine you'd only need one master camera
	/// </summary>
	[RequireComponent(typeof(Camera))]
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
		public CameraZoom cameraZoom => _cz;

		//Members
		Camera _cam = null;
		CameraInput _ci = null;
		CameraMover _cm = null;
		CameraZoom _cz = null;

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
			_cam = GetComponent<Camera>();
			_ci = _cam.GetComponent<CameraInput>();
			_cm = _cam.GetComponent<CameraMover>();
			_cz = _cam.GetComponent<CameraZoom>();
		}

		//Returns the attached camera
		// public static imp licit operator Camera(MasterCamera rhs) => rhs.cam;
	}
}