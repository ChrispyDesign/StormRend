using UnityEngine;

namespace StormRend.Systems.StateMachines
{
    public abstract class State : MonoBehaviour
    {
        internal virtual void OnEnter() { }

        internal virtual void OnUpdate(CoreStateMachine sm) { }

        internal virtual void OnExit() { }
    }
}