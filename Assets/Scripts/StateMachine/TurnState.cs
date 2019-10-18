using StormRend.Defunct;
using StormRend.States.UI;
using StormRend.Systems.StateMachines;
using StormRend.Utility.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace StormRend.States
{
    public abstract class TurnState : UIState
    {
        [Header("Stats")]
        [ReadOnlyField] public float turnCount = 0;
        [ReadOnlyField] public float currentStateTime;
        [ReadOnlyField] public float longestStateTime = 0;
        [ReadOnlyField] public float totalStateTime = 0;

        [Header("Events")]
		[SerializeField] protected UnityEvent OnTurnEnter;
        [SerializeField] protected UnityEvent OnTurnExit;

        /// NOTE! All these methods must be called by overridden methods to preserve correct functionality

        public override void OnEnter(UltraStateMachine sm)
        {
            base.OnEnter(sm);

            OnTurnEnter.Invoke();

            //Stats
            turnCount++;
            currentStateTime = 0;
        }

        public override void OnUpdate(UltraStateMachine sm)
        {
            currentStateTime += Time.deltaTime;
        }

        public override void OnExit(UltraStateMachine sm)
        {
            base.OnExit(sm);
            // Debug.Log("OnTurnExit()");
            OnTurnExit.Invoke();

            //Update longest turn
            if (currentStateTime > longestStateTime)
                longestStateTime = currentStateTime;

            totalStateTime += currentStateTime;
        }

        //Auto handle pause and unpause
        public override void OnCover(UltraStateMachine sm)
        {
            base.OnCover(sm);     //Hides UI

            Time.timeScale = 0;
        }

        public override void OnUncover(UltraStateMachine sm)
        {
            base.OnUncover(sm);   //Unhide UI

            Time.timeScale = 1f;
        }

    }
}