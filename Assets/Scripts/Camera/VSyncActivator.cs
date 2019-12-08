/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

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