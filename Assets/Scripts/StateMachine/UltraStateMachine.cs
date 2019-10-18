﻿using System;
using System.Collections.Generic;
using StormRend.Utility.Attributes;
using StormRend.Utility.Events;
using UnityEngine;
using UnityEngine.Events;

namespace StormRend.Systems.StateMachines
{
	/// <summary>
	/// Turn-based stackable state machine.
	/// Turn states for turn based games.
	/// Stacks for Pause menus etc.
	/// </summary>
	public sealed class UltraStateMachine : MonoBehaviour //: Singleton<UltraStateMachine>
	{
		//Normally this state machine starts of running states in turnStates
		//If a state is stacked, then the current state is saved to be returned to later
		//and the state machines starts using stack states
		//Turn based states can only be returned to once all Stacked states have bee popped off

	#region Inspector
		[ReadOnlyField] [SerializeField] int _currentStateIDX = 0;
		[SerializeField] State entryState;
		[SerializeField] List<State> turnStates = new List<State>();  //They have to be StackStates because they can be covered/uncovered
		Stack<State> stackStates = new Stack<State>();

		[Space]
		public StateEvent OnNextTurn;
		// public static Action<State> onNextTurn;		//CAREFUL: static
	#endregion
	#region Properties
		public int turnsCount => turnStates.Count;
		public int stackCount => stackStates.Count;
		public State currentState
		{
			get
			{
				//If stack populated then return top most stacked state
				if (isInStackMode)
					return stackStates.Peek();
				//else return current turn state
				else if (isInTurnBasedMode)
					return turnStates[currentStateIDX];
				//return null if neither collection has any states
				else
					return null;
			}
		}
		int currentStateIDX
		{
			get => _currentStateIDX;
			set
			{
				_currentStateIDX = value;

				//Wrap around
				if (_currentStateIDX > turnStates.Count - 1)
					_currentStateIDX = 0;
				else if (_currentStateIDX < 0)
					_currentStateIDX = turnStates.Count - 1;
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
				enabled = false;
			}
			else
			{
				//Insert into turn states (no duplicates)
				Insert(entryState);
				//Enter entry state
				entryState.OnEnter(this);
				//Set initial turn index
				currentStateIDX = turnStates.IndexOf(entryState);
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
				//Exit current state
				currentState?.OnExit(this);
				//Switch to new state
				turnStates[currentStateIDX] = state;
				//Enter new state
				currentState.OnEnter(this);
			}
			else
			{
				Debug.LogWarning("Cannot switch in stack mode OR if turn state list is empty");
			}
		}
	#endregion
	#region Turn-Based
		/// <summary>
		/// Inserts a new state into list of turn states. Will not allow duplicate states. States can be inserted anytime.
		/// </summary>
		public void Insert(State turnState)
		{
			if (!turnStates.Contains(turnState))    //No duplicates
				turnStates.Add(turnState);
		}
		/// <summary>
		/// Removes specified state from list of turn states. States can be removed anytime.
		/// </summary>
		public void Remove(State turnState)
		{
			turnStates.Remove(turnState);
		}

		/// <summary>
		/// Clears list of turn states. States can be cleared anytime.
		/// </summary>
		public void ClearTurns()
		{
			turnStates.Clear();
		}

		/// <summary>
		/// Select next turn state
		/// </summary>
		public void NextTurn()
		{
			OnNextTurn.Invoke(currentState);

			//Can only go to next turn if in turn based mode
			if (isInTurnBasedMode)
			{
				//Exit current state
				currentState?.OnExit(this);
				//Set next state index
				currentStateIDX++;
				//Enter next state
				currentState?.OnEnter(this);
			}
		}

		/// <summary>
		/// Select previous turn state. Probably impractical
		/// </summary>
		public void PrevTurn()
		{
			if (isInTurnBasedMode)
			{
				currentState?.OnExit(this);
				currentStateIDX--;
				currentState?.OnEnter(this);
			}
		}
	#endregion
	#region Stacks
		//------------ Stackable --------------
		/// <summary>
		/// Stacks a state on top of current state. If state is a turn state then it will switch to Stack State Mode
		/// and Next/PrevTurn() cannot be called until all states in the stack are popped. Stack state duplicates allowed.
		/// </summary>
		public void Stack(State state)
		{
			// Debug.Log("Stacking: " + state.GetType().Name);

			//Cover current state
			currentState?.OnCover(this);
			//Push on new state
			stackStates.Push(state);
			//Enter new state
			currentState?.OnEnter(this);
		}

		public void UnStack()
		{
			if (isInStackMode)
			{
				// Debug.Log("Un-stacking: " + currentState.GetType().Name);

				//Exit current stack state
				currentState?.OnExit(this);
				//Push off current state (automatically setting the new state)
				stackStates.Pop();
				//Uncover state below
				currentState?.OnUncover(this);
			}
			else
			{
				Debug.LogWarning("Nothing to unstack");
			}
		}

		public void ClearStack()
		{
			//Keep unstacking, while executing appropriate state methods, until the stack is clear
			while (isInStackMode)
				UnStack();
		}
	#endregion
	}
}