using StormRend.Systems.StateMachines;
using UnityEngine;

namespace StormRend.States.UI
{
	/// <summary>
	/// Automatically reveals on state enter and uncover, hide on state exit and cover
	/// </summary>
	public abstract class UIState : State
	{
		[Tooltip("The UI elements to display and activate on entering or uncovering of this state")]
		[SerializeField] GameObject[] uiItems;

		///NOTE! All these methods must be called from override methods to preserve correct functionality

		public override void OnEnter(UltraStateMachine sm) => SetUIActive(true);
		public override void OnUncover(UltraStateMachine sm) => SetUIActive(true);
		public override void OnExit(UltraStateMachine sm) => SetUIActive(false);
		public override void OnCover(UltraStateMachine sm) => SetUIActive(false);

		void SetUIActive(bool active)
		{
			foreach (var i in uiItems)
				i?.SetActive(active);
		}
	}
}
