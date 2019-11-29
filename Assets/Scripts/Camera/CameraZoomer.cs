using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace StormRend.CameraSystem
{
	[RequireComponent(typeof(CameraInput))]
	public class CameraZoomer : MonoBehaviour
	{
		//Inspector
		// [SerializeField] 
		[SerializeField] float zoomLerp = 0.2f;
		[SerializeField] float zoomSpeed = 1f;
		[SerializeField] float minZoom = 0.025f;
		[SerializeField] float maxZoom = 2f;

		[Header("Depth of Field")]
		//Good settings: Aperture f3.2, Focal Length 177mm, Max blur size Medium
		[SerializeField] bool DOFOn = true;
		[SerializeField] float focusDistOffset = 1.5f;
		[SerializeField] float focusLerp = 0.1f;


		//Members
		CameraInput input = null;
		new Camera camera = null;
		Vector3 desiredZoomPos = Vector3.one;
		Vector3 cameraInitLocalOffset;
		PostProcessProfile ppp = null;
		float zoomFactor = 1f;
		DepthOfField dof = null;
		float desiredFocusDist;
		bool oldDOFon = true;

		void Awake()
		{
			input = GetComponent<CameraInput>();
		}

		void Start()
		{
			camera = GetComponentInChildren<Camera>();
			desiredZoomPos = cameraInitLocalOffset = camera.transform.localPosition;
			camera.transform.LookAt(transform.position, Vector3.up);
			ppp = GetComponent<PostProcessVolume>().profile;
			if (!ppp.TryGetSettings<DepthOfField>(out dof))
				Debug.LogWarning("No Depth of Field found on Post Processing Stack!");

			//Calibrate for the first time
			SetDesiredZoomAndFocus();

			oldDOFon = DOFOn;
		}

		void Update()
		{
			if (oldDOFon != DOFOn)
			{
				dof.enabled.value = DOFOn;
				SetDesiredZoomAndFocus();
				oldDOFon = DOFOn;
			}

			if (input.zoomAxis != 0)
				SetDesiredZoomAndFocus();
		}

		void LateUpdate()
		{
			//Lerp
			camera.transform.localPosition = Vector3.Lerp(camera.transform.localPosition, desiredZoomPos, zoomLerp);
			if (DOFOn) dof.focusDistance.value = Mathf.Lerp(dof.focusDistance.value, desiredFocusDist, focusLerp) - focusDistOffset;
		}

		void SetDesiredZoomAndFocus()
		{
			zoomFactor -= input.zoomAxis * zoomSpeed;
			zoomFactor = Mathf.Clamp(zoomFactor, minZoom, maxZoom);
			desiredZoomPos.x = cameraInitLocalOffset.x * zoomFactor;
			desiredZoomPos.y = cameraInitLocalOffset.y * zoomFactor;
			desiredZoomPos.z = cameraInitLocalOffset.z * zoomFactor;

			if (DOFOn) desiredFocusDist = Vector3.Dot(transform.position - camera.transform.position, camera.transform.forward);
			// if (focusOn) desiredFocusDist = Vector3.Distance(transform.position, camera.transform.position);
			// Debug.Log(desiredFocusDist);
		}

		// void OnDrawGizmos()	//Trying to figure out where everything is
		// {
		// 	Gizmos.DrawSphere(camera.transform.position, 0.5f);
		// 	Gizmos.DrawSphere(transform.position, 0.5f);
		// }
	}
}