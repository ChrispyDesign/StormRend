using System.Collections;
using System.Collections.Generic;
using StormRend.Utility.Attributes;
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
        [ReadOnlyField][SerializeField] int _currentTurnStateIDX;
        int currentTurnStateIDX
        {
            get => _currentTurnStateIDX;
            set
            {
                _currentTurnStateIDX = value;

                //Wrap around
                if (_currentTurnStateIDX > turnStates.Count - 1)
                    _currentTurnStateIDX = 0;
                else if (_currentTurnStateIDX < 0)
                    _currentTurnStateIDX = turnStates.Count - 1;
            }
        }

        public bool isInStackMode => stackStates.Count > 0;
        public bool isInTurnBasedMode => !isInStackMode;
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
                //Insert while taking care of duplicates
                Insert(entryState);

                //Execute OnEnter()
                Switch(entryState);

                //Set initial turn index
                currentTurnStateIDX = turnStates.IndexOf(entryState);
            }
        }
        #endregion

        //------------- Turn-Based -------------
        /// <summary>
        /// Inserts a new state into list of turn states. Will not allow duplicate states.
        /// States can be inserted anytime.
        /// </summary>
        public void Insert(StackState turnState)
        {
            if (!turnStates.Contains(turnState))    //No duplicates
                turnStates.Add(turnState);
        }
        /// <summary>
        /// Removes specified state from list of turn states.
        /// States can be removed anytime.
        /// </summary>
        public void Remove(StackState turnState) => turnStates.Remove(turnState);

        /// <summary>
        /// Switch to the next turn state
        /// </summary>
        public void NextTurn()
        {
            if (isInTurnBasedMode) Switch(turnStates[currentTurnStateIDX++]);
        }
        /// <summary>
        /// Switch to the previous turn state
        /// </summary>
        public void PrevTurn()
        {
            if (isInTurnBasedMode) Switch(turnStates[currentTurnStateIDX--]);
        }

        //------------ Stackable --------------
        /// <summary>
        /// Stacks a state on top of current state. If state is a turn state then it will switch to Stack State Mode
        /// and Next/PrevTurn() cannot be called until all states in the stack are popped.
        /// </summary>
        /// <param name="state"></param>
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

        public void UnStack()
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
            }
            else
            {
                Debug.LogWarning("Nothing to unstack");
            }
        }
    }
}