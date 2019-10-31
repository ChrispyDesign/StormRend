using StormRend.Systems.StateMachines;
using UnityEngine;

namespace StormRend.States.UI
{
	/// <summary>
	/// Automatically activates objects on state enter and uncover, deactivate on state exit and cover
	/// </summary>
	public class OnState : State
	{
		[Tooltip("The UI/GameObjects to activate and deactivate on entering and exiting state, respectively")]
		[SerializeField] GameObject[] objectsToActivate = null;

		///NOTE! All these methods must be called from override methods to preserve correct functionality

		public override void OnEnter(UltraStateMachine sm) => SetObjectsActive(true);
		public override void OnUncover(UltraStateMachine sm) => SetObjectsActive(true);
		public override void OnExit(UltraStateMachine sm) => SetObjectsActive(false);
		public override void OnCover(UltraStateMachine sm) => SetObjectsActive(false);

		void SetObjectsActive(bool active)
		{
			foreach (var i in objectsToActivate)
				i?.SetActive(active);
		}
	}
}
