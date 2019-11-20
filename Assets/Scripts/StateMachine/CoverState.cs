using StormRend.Systems.StateMachines;
using UnityEngine;

namespace StormRend.States
{
	/// <summary>
	/// Automatically covers current state with specified objects on enter. Uncovers on exit
	/// </summary>
	public class CoverState : State
	{
		[Tooltip("Objects that will cover the current state on state enter")]
		[SerializeField] GameObject[] coveringObjects = null;

		///NOTE! All these methods must be called from override methods to preserve correct functionality
		public override void OnEnter(UltraStateMachine sm) => SetObjectsActive(true);
		public override void OnUncover(UltraStateMachine sm) => SetObjectsActive(true);
		public override void OnExit(UltraStateMachine sm) => SetObjectsActive(false);
		public override void OnCover(UltraStateMachine sm) => SetObjectsActive(false);

		void SetObjectsActive(bool active)
		{
			foreach (var i in coveringObjects)
				i?.SetActive(active);
		}
	}
}
