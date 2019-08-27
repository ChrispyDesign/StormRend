using UnityEngine;

namespace StormRend.Prototype
{
	public class CameraFacer : MonoBehaviour
	{
		private void Update()
		{
			transform.LookAt(Camera.main.transform);
		}
	}
}
