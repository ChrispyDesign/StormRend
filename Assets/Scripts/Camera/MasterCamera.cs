using UnityEngine;

namespace StormRend.CameraSystem
{
	/// <summary>
	/// This is used as a tag to find locate the main working camera
	/// </summary>
	[RequireComponent(typeof(Camera))]
	public class MasterCamera : MonoBehaviour
	{
		public int priority = 10;
		new Camera camera { get; set; } = null;
		void Awake()
		{
			camera = GetComponentInChildren<Camera>();	//Find below
			if (camera) return;
			camera = GetComponentInParent<Camera>();	//Find above
			if (camera) return;
			Debug.Assert(camera, "No camera found on this object!");
		}

		//Returns the attached camera
		public static implicit operator Camera(MasterCamera rhs) => rhs.camera;
	}
}