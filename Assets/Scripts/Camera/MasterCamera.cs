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
		public Camera linkedCamera 
		{
			get 
			{
				if (!_linkedCamera) Reset();
				return _linkedCamera;
			}
			private set => linkedCamera = value;
		}
		Camera _linkedCamera;

		void Reset()
		{
			_linkedCamera = GetComponent<Camera>();
			if (_linkedCamera) return;
			_linkedCamera = GetComponentInChildren<Camera>();	//Find below
			if (_linkedCamera) return;
			_linkedCamera = GetComponentInParent<Camera>();		//Find above
			Debug.Assert(_linkedCamera, "No camera found on this object!");
		}

		//Returns the attached camera
		public static implicit operator Camera(MasterCamera rhs) => rhs.linkedCamera;
	}
}