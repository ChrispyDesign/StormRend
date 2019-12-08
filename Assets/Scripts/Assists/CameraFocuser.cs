/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

using StormRend.Abilities;
using StormRend.CameraSystem;
using StormRend.MapSystems.Tiles;
using StormRend.Units;
using UnityEngine;

namespace StormRend.Assists
{
    public class CameraFocuser : MonoBehaviour
    {
		//Inspector
		[SerializeField] float lerpTime = 1f;

		//Members
		CameraMover cameraMover = null;

		void Awake() => cameraMover = MasterCamera.current.cameraMover;

		void Start()
		{
			if (!cameraMover)
			{
				Debug.LogWarning("Camera mover not found! Disabling...");
				enabled = false;
			}
		}

		public void Focus(Tile target) => cameraMover.Move(target, lerpTime);
		public void Focus(Unit target) => cameraMover.Move(target, lerpTime);
		public void Focus(Vector3 target) => cameraMover.Move(target, lerpTime);
		public void Focus(Ability a) => cameraMover.Move(a.lastTargetPos, lerpTime);
	}
}