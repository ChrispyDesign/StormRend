using StormRend.States.UI;
using StormRend.Systems.StateMachines;
using StormRend.Utility.Attributes;
using UnityEngine;

namespace StormRend.States
{
    public abstract class TurnState : UIState
    {
        //Record the time
        [Header("Timing")]
        [ReadOnlyField] public float currentTurnTime;
        [ReadOnlyField] public float longestTurnTime = 0;
        [ReadOnlyField] public float totalTurnTime = 0;

        internal override void OnEnter()
        {
            currentTurnTime = 0;
        }

        internal override void OnUpdate(CoreStateMachine sm)
        {
            currentTurnTime += Time.deltaTime;
        }

        internal override void OnExit()
        {
            //Update longest turn
            if (currentTurnTime > longestTurnTime)
                longestTurnTime = currentTurnTime;

            totalTurnTime += currentTurnTime;
        }

        //Auto handle pause and unpause
        internal override void OnCover()
        {
            base.OnCover();     //Hides UI

            Time.timeScale = 0;
        }

        internal override void OnUncover()
        {
            base.OnUncover();   //Unhide UI

            Time.timeScale = 1f;
        }

    }
}