using System.Collections;
using StormRend.Abilities;
using StormRend.Systems;
using StormRend.Systems.StateMachines;
using StormRend.UI;
using StormRend.Units;
using UnityEngine;

namespace StormRend.Assists
{
	/// <summary>
	/// Checks if all ally units have taken all their actions
	/// If so, start the end turn process and timer
	/// </summary>
	[RequireComponent(typeof(UnitRegistry))]
	public class AllActionsUsedChecker : MonoBehaviour
	{
		//Inspector
		[SerializeField] float delay = 3f;
		[SerializeField] EndTurnTimer endTurnTimer = null;

		//Properties

		/// <summary>
		/// Returns true if none of the ally units can either move or act anymore
		/// </summary>
		public bool isAllActionsUsedUp
		{
			get 
			{
				foreach (var ally in ur.GetAliveUnitsByType<AllyUnit>())
				{
					//Invalidates if a single unit can still move or take action
					if (ally.canMove || ally.canAct) return false;
				}
				return true;
			}
		}

		//Members
		UltraStateMachine usm = null;
		UnitRegistry ur = null;
		GameDirector gd = null;

		void Awake()
		{
			ur = GetComponent<UnitRegistry>();
			usm = FindObjectOfType<UltraStateMachine>();
			gd = GameDirector.current;
			if (!endTurnTimer) endTurnTimer = FindObjectOfType<EndTurnTimer>();

			Debug.Assert(usm, "No Ultra State Machine found!");
			Debug.Assert(endTurnTimer, "No turn end countdown timer passed in!");
		}

		//Register for each unit's onActed events
		void Start()    //OnEnable runs too early
		{
			endTurnTimer.gameObject.SetActive(false);

			foreach (var u in ur.GetAliveUnitsByType<AllyUnit>())
			{
				var au = u as AnimateUnit;
				au.onActed.AddListener(CheckCompletedActions);
			}
		}
		void OnDisable()
		{
			foreach (var u in ur.GetAliveUnitsByType<AllyUnit>())
			{
				var au = u as AnimateUnit;
				au.onActed.RemoveListener(CheckCompletedActions);
			}
		}

		/// <summary>
		/// Checks if all the actions have been used up
		/// </summary>
		public void CheckCompletedActions(Ability a)
		{
			if (isAllActionsUsedUp) 
				//No units can no longer act > all actions used up > end turn
				StartCoroutine(NextTurn(delay));
		}

		/// <summary>
		/// Stops the timer immediately
		/// </summary>
		public void Stop()
		{
			StopAllCoroutines();
			endTurnTimer?.gameObject.SetActive(false);
		}

		/// <summary>
		/// Starts the end turn timer and automatically switches to the next turn once done
		/// </summary>
		IEnumerator NextTurn(float delay)
		{
			//Activate counter
			endTurnTimer?.gameObject.SetActive(true);

			float time = delay;
			while (time > 0)
			{
				time -= Time.deltaTime;
				endTurnTimer?.SetTime(time);
				yield return null;
			}

			//NOTE!!! This might be the reason why enemies sometimes "double hit"
			//If the last ally attack pushed an enemy off the edge, 
			//"usm.NextTurn()" would trigger too early and somehow cause an extra turn to happen
			//Bloody coroutines

			gd.SafeNextTurn();		
			// usm.NextTurn();

			//Deactivate counter
			endTurnTimer?.gameObject.SetActive(false);
		}
	}
}