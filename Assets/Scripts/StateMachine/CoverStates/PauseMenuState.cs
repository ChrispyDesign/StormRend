using UnityEngine;
using StormRend.Systems.StateMachines;

namespace StormRend.States
{
    /// <summary>
    /// * Auto handles game timescale changes
    /// </summary>
    public class PauseMenuState : CoverState
    {
        [Space(10), SerializeField] float pauseTimeScale = 0.2f;

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