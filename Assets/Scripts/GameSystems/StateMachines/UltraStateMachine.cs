using System.Collections;
using System.Collections.Generic;
using BhaVE.Patterns;
using StormRend.Utility.Attributes;
using UnityEngine;

namespace StormRend.Systems.StateMachines
{
	/// <summary>
	/// Turn-based stackable state machine.
	/// Turn-based allows for improved NextTurn() API.
	/// Stackable for Pause menus etc.
	/// </summary>
	public abstract class UltraStateMachine : Singleton<UltraStateMachine>
	{
		//Normally this state machine starts of running states in turnStates
		//If a state is stacked, then the current state is saved to be returned to later
		//and the state machines starts using stack states
		//Turn based states can only be returned to once all Stacked states have bee popped off
		[SerializeField] State entryState;
		[SerializeField] List<State> turnStates = new List<State>();  //They have to be StackStates becaused they can be covered/uncovered
		Stack<State> stackStates = new Stack<State>();

		#region Properties
		public State currentState
		{
			get
			{
				//If stack populated then return top most stacked state
				if (isInStackMode)
					return stackStates.Peek();
				//else return current turn state
				else if (isInTurnBasedMode)
					return turnStates[currentTurnStateIDX];
				//return null if neither collection has any states
				else
					return null;
			}
		}
		[ReadOnlyField] [SerializeField] int _currentTurnStateIDX = 0;
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
		public bool isInTurnBasedMode => !isInStackMode && turnStates.Count > 0;
		#endregion

		#region Core
		void Start()
		{
			if (!entryState)
			{
				Debug.LogWarning("No entry state found!");
				this.enabled = false;
			}
			else
			{
				//Insert into turn states (no duplicates)
				Insert(entryState);

				//Enter entry state
				Switch(entryState);

				//Set initial turn index
				currentTurnStateIDX = turnStates.IndexOf(entryState);
			}
		}

		void Update()
		{
			currentState?.OnUpdate(this);
		}

		/// <summary>
		/// Switch current turn state with a new one
		/// </summary>
		public void Switch(State state)
		{
			if (isInTurnBasedMode)
			{
				currentState?.OnExit(this);
				turnStates[currentTurnStateIDX] = state;
				state?.OnEnter(this);
			}
			else
			{
				Debug.LogWarning("Cannot switch in stack mode OR if turn state list is empty");
			}
		}
		#endregion

		#region Turn-Based
		/// <summary>
		/// Inserts a new state into list of turn states. Will not allow duplicate states.
		/// States can be inserted anytime.
		/// </summary>
		public virtual void Insert(State turnState)
		{
			if (!turnStates.Contains(turnState))    //No duplicates
				turnStates.Add(turnState);
		}
		/// <summary>
		/// Removes specified state from list of turn states.
		/// States can be removed anytime.
		/// </summary>
		public virtual void Remove(State turnState)
		{
			turnStates.Remove(turnState);
		}

		/// <summary>
		/// Select next turn state
		/// </summary>
		public virtual void NextTurn()
		{
			if (isInTurnBasedMode) Switch(turnStates[currentTurnStateIDX++]);
		}
		/// <summary>
		/// Select previous turn state
		/// </summary>
		public virtual void PrevTurn()
		{
			if (isInTurnBasedMode) Switch(turnStates[currentTurnStateIDX--]);
		}
		#endregion

		#region Stacks
		//------------ Stackable --------------
		/// <summary>
		/// Stacks a state on top of current state. If state is a turn state then it will switch to Stack State Mode
		/// and Next/PrevTurn() cannot be called until all states in the stack are popped. Duplicates allowed.
		/// </summary>
		public virtual void Stack(State state)
		{
			//Cover current state
			currentState.OnCover(this);

			//Enter new state
			state.OnEnter(this);

			//Set current state
			stackStates.Push(state);
		}

		public virtual void UnStack()
		{
			if (isInStackMode)
			{
				//Exit current state
				currentState.OnExit(this);

				//Pop stack (automatically setting the new state)
				stackStates.Pop();

				//Uncover state
				currentState.OnUncover(this);
			}
			else
			{
				Debug.LogWarning("Nothing to unstack");
			}
		}
		#endregion
	}
}