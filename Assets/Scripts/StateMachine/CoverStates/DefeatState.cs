using StormRend.CameraSystem;
using StormRend.Systems.StateMachines;
using StormRend.Units;
using UnityEngine;

namespace StormRend.States
{
	public class DefeatState : CoverState
	{
		[SerializeField] float cameraCenteringSpeed = 10f;
		CameraMover cm;
		UnitRegistry ur;

		public override void OnEnter(UltraStateMachine sm)
		{
			base.OnEnter(sm);

			cm = MasterCamera.current.GetComponent<CameraMover>();
			ur = UnitRegistry.current;

			//Move camera to the average position of all the units
			var aliveAllies = ur.GetAliveUnitsByType<AllyUnit>();
			Vector3 avgPos = Vector3.zero;
			foreach (var a in aliveAllies)
				avgPos += a.transform.position;
			avgPos /= (float)aliveAllies.Length;
			cm.MoveTo(avgPos, cameraCenteringSpeed);

			//Prevent user from further moving it
			cm.enabled = false;
		}
	}
}