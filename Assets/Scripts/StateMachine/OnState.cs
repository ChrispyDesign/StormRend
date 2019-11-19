using StormRend.Systems.StateMachines;
using UnityEngine;

namespace StormRend.States
{
	/// <summary>
	/// Automatically activates specified objects on enter without covering. Deactivates on exit
	/// </summary>
	public class OnState : State
	{
		[Tooltip("Objects that will be activated on state enter")]
		[SerializeField] GameObject[] activatingObjects = null;

		public override void OnEnter(UltraStateMachine sm) => SetObjectsActive(true);
		public override void OnExit(UltraStateMachine sm) => SetObjectsActive(false);

		void SetObjectsActive(bool active)
		{
			foreach (var i in activatingObjects)
				i?.SetActive(active);
		}
	}
}