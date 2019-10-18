using UnityEngine;
using StormRend.Systems.StateMachines;

namespace StormRend.States.UI
{
    /// <summary>
    /// * Auto handles game timescale changes
    /// </summary>
    public class PauseMenuState : UIState
    {
        [SerializeField] float pauseTimeScale = 0.3f;

        public override void OnEnter(UltraStateMachine sm)
        {
            base.OnEnter(sm);

            Time.timeScale = pauseTimeScale;
        }

        public override void OnExit(UltraStateMachine sm)
        {
            base.OnExit(sm);

            Time.timeScale = 1f;
        }
    }
}