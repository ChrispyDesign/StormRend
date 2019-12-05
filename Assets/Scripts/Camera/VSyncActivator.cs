using UnityEngine;

namespace StormRend.CameraSystem 
{ 
	public class VSyncActivator : MonoBehaviour
	{
		[SerializeField] int targetFPS = 60;
		[SerializeField] int queuedFrames = 3;
		[SerializeField] int vSync = 2;

        void Start()
		{
			//Triple buffer?
			QualitySettings.maxQueuedFrames = queuedFrames;
			QualitySettings.vSyncCount = vSync;
			Application.targetFrameRate = targetFPS;

            Cursor.lockState = CursorLockMode.Confined;
		}
   	}
}