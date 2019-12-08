/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

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