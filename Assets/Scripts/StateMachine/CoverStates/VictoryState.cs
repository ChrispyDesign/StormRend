/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

using StormRend.CameraSystem;
using StormRend.Systems.StateMachines;
using StormRend.Units;
using UnityEngine;

namespace StormRend.States
{
	public class VictoryState : NarrativeState
	{
		[SerializeField] float cameraCenteringSpeed = 10f;
		MasterCamera cam;
		UnitRegistry ur;

		public override void OnEnter(UltraStateMachine sm)
		{
			base.OnEnter(sm);

			cam = MasterCamera.current;
			ur = UnitRegistry.current;

			//Move camera to the average position of all the units
			var aliveAllies = ur.GetAliveUnitsByType<AllyUnit>();
			Vector3 avgPos = Vector3.zero;
			foreach (var a in ur.aliveUnits)
				avgPos += a.transform.position;
			avgPos /= (float)ur.aliveUnits.Length;
			cam.cameraMover.Move(avgPos, cameraCenteringSpeed);

			//Prevent user from further moving it
			cam.cameraInput.enabled = false;	
		}
	}
}