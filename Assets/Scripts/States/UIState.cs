using StormRend.Systems.StateMachines;
using UnityEngine;

namespace StormRend.States.UI
{
    /// <summary>
    /// Automatically reveals on state enter and uncover, hide on state exit and cover
    /// </summary>
    public abstract class UIState : State
    {
		[SerializeField] GameObject[] uiItems;
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
