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
		public int priority = 10;
		public new Camera camera 
		{
			get 
			{
				if (!_cam) Reset();
				return _cam;
			}
			private set => camera = value;
		}
		Camera _cam;

		void Reset()
		{
			_cam = GetComponent<Camera>();
			if (_cam) return;
			_cam = GetComponentInChildren<Camera>();	//Find below
			if (_cam) return;
			_cam = GetComponentInParent<Camera>();		//Find above
			Debug.Assert(_cam, "No camera found on this object!");
		}

		//Returns the attached camera
		// public static imp licit operator Camera(MasterCamera rhs) => rhs.cam;
	}
}