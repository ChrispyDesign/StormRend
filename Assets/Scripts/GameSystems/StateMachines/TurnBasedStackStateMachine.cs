using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StormRend.Systems.StateMachines
{
    /// <summary>
    /// Turn-based stackable state machine. 
    /// Turn-based allows for improved NextTurn() API.
    /// Stackable for Pause menus etc.
    /// </summary>
    public class TurnBasedStackStateMachine : CoreStateMachine
    {
        //Normally this state machine starts of running states in turnStates
        //If a state is stacked, then the current state is saved to be returned to later
        //and the state machines starts using stack states
        //Turn based states can only be returned to once all Stacked states have bee popped off

        public override State currentState
        {
            get
            {
                if (stackStates.Count > 0)
                    return stackStates.Peek();
                else if (turnStates.Count > 0)
                    return turnStates[currentTurnStateIdx];
                else
                    return null;
            }
            private set;
        }

        [SerializeField] State entryState;
        [SerializeField] List<State> turnStates = new List<State>();
        int currentTurnStateIdx {
            get { return currentTurnStateIdx; }
            set
            {
                if (currentTurnStateIdx > turnStates?.Count-1)
                    currentTurnStateIdx = 0;
                else if (currentTurnStateIdx < 0)
                    currentTurnStateIdx = turnStates.Count-1;
            }
        }

        Stack<State> stackStates = new Stack<State>();

        //The state the will be returned to once all the states are popped off
        State returnToState;

        public bool isInStackMode => stackStates.Count > 0;
        public bool isInTurnMode => !isInStackMode;


        void Start()
        {
            if (!entryState)
            {
                Debug.LogWarning("No initial state found!");
                this.enabled = false;
            }
            else
            {
                //If state is not already inside 
            }
        }

        //------------- Turn-Based -------------
        public void Insert(State turnState)
        {
            turnStates.Add(turnState);
        }
        public void Remove(State turnState)
        {
            turnStates.Remove(turnState);
        }

        public void NextTurn()
        {
            currentState = turnStates[++currentTurnStateIdx];
        }

        //------------ Stackable --------------
        public void Stack(State stackState)
        {
            stackStates.Push(stackState);
        }
        public State UnStack()
        {
            if (isInStackMode)
                return stackStates.Peek();
            else
            {
                Debug.LogWarning("No states to un stack!");
                return null;
            }
        }
    }
}