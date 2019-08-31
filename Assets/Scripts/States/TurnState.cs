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

		//General
        [Header("Events")]
		[SerializeField] protected UnityEvent OnTurnEnter;
        [SerializeField] protected UnityEvent OnTurnExit;

		GameManager gm;
        BlizzardController bc;

        void Awake()
        {
            //TODO try to not use singletons
            gm = GameManager.singleton;
            bc = UIManager.GetInstance().GetBlizzardManager();
        }

        public override void OnEnter(UltraStateMachine sm)
        {
            OnTurnEnter.Invoke();

            //Stats
            turnCount++;
            currentStateTime = 0;

            //Blizzard
            bc.IncrementBlizzardMeter();
        }

        public override void OnUpdate(UltraStateMachine sm)
        {
            currentStateTime += Time.deltaTime;
        }

        public override void OnExit(UltraStateMachine sm)
        {
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