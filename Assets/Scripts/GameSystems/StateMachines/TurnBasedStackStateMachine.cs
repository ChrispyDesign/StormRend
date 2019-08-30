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
    public abstract class TurnBasedStackStateMachine : CoreStateMachine
    {
        //Normally this state machine starts of running states in turnStates
        //If a state is stacked, then the current state is saved to be returned to later
        //and the state machines starts using stack states
        //Turn based states can only be returned to once all Stacked states have bee popped off
        [SerializeField] StackState entryState;
        [SerializeField] List<StackState> turnStates = new List<StackState>();  //They have to be StackStates becaused they can be covered/uncovered
        Stack<StackState> stackStates = new Stack<StackState>();

        #region  Properties
        public override State currentState
        {
            get
            {
                if (stackStates.Count > 0)
                    return stackStates.Peek();
                else if (turnStates.Count > 0)
                    return turnStates[currentTurnStateIDX];
                else
                    return null;
            }
        }
        public bool isInStackMode => stackStates.Count > 0;
        public bool isInTurnBasedMode => !isInStackMode;

        int _currentTurnStateIDX;
        int currentTurnStateIDX
        {
            get => _currentTurnStateIDX;
            set
            {
                _currentTurnStateIDX = value;

                //Wrap around
                if (_currentTurnStateIDX > turnStates.Count-1)
                    _currentTurnStateIDX = 0;
                else if (_currentTurnStateIDX < 0)
                    _currentTurnStateIDX = turnStates.Count-1;
            }
        }
        #endregion

        //The state the will be returned to once all the states are popped off
        State coveredTurnState;

        #region Core
        void Start()
        {
            if (!entryState)
            {
                Debug.LogWarning("No initial state found!");
                this.enabled = false;
            }
            else
            {
                //Add entry state to turn states and set the turn index
                Insert(entryState);
                currentTurnStateIDX = turnStates.IndexOf(entryState);
            }
        }
        #endregion

        //------------- Turn-Based -------------
        /// <summary>
        /// Inserts a new state inside the list of turn states.
        /// Does not allow for duplicates
        /// </summary>
        public void Insert(StackState turnState)
        {
            if (!turnStates.Contains(turnState))    //No duplicates
                turnStates.Add(turnState);
        }
        public void Remove(StackState turnState)
        {
            turnStates.Remove(turnState);
        }

        public void NextTurn()
        {
            if (isInTurnBasedMode)
            {
                currentState = turnStates[currentTurnStateIDX++];
            }
        }

        //------------ Stackable --------------
        public void Stack(StackState state)
        {
            //Cover current state
            (currentState as StackState).OnCover();

            if (isInTurnBasedMode)
            {
                //Deal with initial turn based state covering
                coveredTurnState = currentState;
            }

            //Set new current state
            currentState = state;
            state.OnEnter();

            //Push onto stack
            stackStates.Push(state);
        }
        public State UnStack()
        {
            if (isInStackMode)
            {
                //Exit current state
                (currentState as StackState).OnExit();

                //Pop stack
                currentState = stackStates.Pop();

                //If last then revert back to turn based mode (ie. if stackStates <= 0)
                if (isInTurnBasedMode)
                {
                    currentState = coveredTurnState;
                }
                
                //Uncover state
                (currentState as StackState).OnUncover();

                return currentState;
            }
            else
            {
                Debug.LogWarning("Nothing to unstack");
                return null;
            }
        }
    }
}