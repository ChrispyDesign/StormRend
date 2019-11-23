using StormRend.Enums;
using StormRend.Systems.StateMachines;
using StormRend.Utility.Attributes;
using StormRend.Utility.Events;
using UnityEngine;

namespace StormRend.States
{
	public class TurnState : CoverState
    {
		[Header("Unit Filtering"), Tooltip("The unit type that can be controlled when game is in this state")]
		public TargetType unitType;
        public bool hasUserControllableUnits = false;

        [Header("Stats")]
        [ReadOnlyField] public float turnCount = 0;
        [ReadOnlyField] public float currentStateTime;
        [ReadOnlyField] public float longestStateTime = 0;
        [ReadOnlyField] public float totalStateTime = 0;

        [Header("Events")]
		[SerializeField] protected StateEvent onTurnEnter;
        [SerializeField] protected StateEvent onTurnExit;

        /// NOTE! All these methods must be called by overridden methods to preserve correct functionality

        public override void OnEnter(UltraStateMachine sm)
        {
            base.OnEnter(sm);

            onTurnEnter.Invoke(this);

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
			
            onTurnExit.Invoke(this);

            //Update longest turn
            if (currentStateTime > longestStateTime)
                longestStateTime = currentStateTime;

            totalStateTime += currentStateTime;
        }
    }
}