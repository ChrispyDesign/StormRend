using System.Collections;
using System.Collections.Generic;
using BhaVE.Patterns;
using StormRend.Utility.Attributes;
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
		[SerializeField] List<State> turnStates = new List<State>();  //They have to be StackStates becaused they can be covered/uncovered
		Stack<State> stackStates = new Stack<State>();

		[Space]
		public UnityEvent OnNextTurn, OnPrevTurn;
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
				Switch(entryState);

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
				currentState?.OnExit(this);
				turnStates[currentStateIDX] = state;
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
			OnNextTurn.Invoke();

			if (isInTurnBasedMode)
				Switch(turnStates[currentStateIDX++]);
		}
		/// <summary>
		/// Select previous turn state
		/// </summary>
		public void PrevTurn()
		{
			OnPrevTurn.Invoke();

			if (isInTurnBasedMode)
				Switch(turnStates[currentStateIDX--]);
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
			Debug.Log("Stacking: " + state.GetType().Name);

			//Cover current state
			currentState.OnCover(this);

			//Set current state
			stackStates.Push(state);

			//Enter new state
			currentState.OnEnter(this);
		}

		public void UnStack()
		{
			if (isInStackMode)
			{
				Debug.Log("Un-stacking: " + currentState.GetType().Name);

				//Exit current stack state
				currentState.OnExit(this);

				// Debug.Log("Pre-Pop().currentState: " + currentState.GetType().Name);
				//Pop stack (automatically setting the new state)
				stackStates.Pop();
				// Debug.Log("Post-Pop().currentState: " + currentState.GetType().Name);

				//Uncover state below
				currentState.OnUncover(this);
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