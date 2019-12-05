using UnityEngine;

namespace StormRend.CameraSystem 
{ 
	public class VSyncActivator : MonoBehaviour
	{
		[SerializeField] int targetFPS = 60;

        void Start()
		{
			QualitySettings.vSyncCount = 0;
			Application.targetFrameRate = targetFPS;

            Cursor.lockState = CursorLockMode.Confined;
		}
   	}
}