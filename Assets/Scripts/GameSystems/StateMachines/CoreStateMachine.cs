using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BhaVE.Patterns;

namespace StormRend.Systems.StateMachines
{
    public class CoreStateMachine : Singleton<CoreStateMachine>
    {
        public virtual State currentState { get; protected set; }

        /// <summary>
        /// Runs every frame
        /// </summary>
        protected virtual void Update()
        {
            currentState?.OnUpdate(this);
        }

        public void Switch(State newState)
        {
            currentState?.OnExit();
            currentState = newState;
            newState?.OnEnter();
        }
    }
}