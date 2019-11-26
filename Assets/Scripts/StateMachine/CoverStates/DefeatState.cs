using StormRend.CameraSystem;
using StormRend.Systems.StateMachines;
using StormRend.Units;
using UnityEngine;

namespace StormRend.States
{
	public class DefeatState : NarrativeState
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