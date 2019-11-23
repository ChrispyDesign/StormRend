using StormRend.CameraSystem;
using StormRend.Systems.StateMachines;
using StormRend.Units;
using UnityEngine;

namespace StormRend.States
{
	public class VictoryState : CoverState
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
			foreach (var a in aliveAllies)
				avgPos += a.transform.position;
			avgPos /= (float)aliveAllies.Length;
			cam.cameraMover.MoveTo(avgPos, cameraCenteringSpeed);

			//Prevent user from further moving it
			cam.cameraInput.enabled = false;	
		}
	}
}