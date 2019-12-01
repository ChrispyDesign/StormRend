using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace StormRend.CameraSystem
{
	[ExecuteInEditMode]
	public sealed class FocusPuller : MonoBehaviour
	{
		//Inspector
		[SerializeField] PostProcessProfile profile;
		[SerializeField] PostProcessVolume volume;
		[SerializeField] new Camera camera = null;
		[SerializeField] Transform target = null;
		[SerializeField] float offset = 0;

		//Members
		DepthOfField dof;
		float originalFocusDistance;

		void Start()
		{
			dof = profile.GetSetting<DepthOfField>();
			originalFocusDistance = dof.focusDistance;

			LateUpdate();
		}

		void LateUpdate()
		{
			if (camera == null || target == null || dof == null) return;

			dof.focusDistance.value = (camera.transform.position - target.position).magnitude + offset;
		}

		void OnDestroy()
		{
			dof.focusDistance.value = originalFocusDistance;
		}
	}
}