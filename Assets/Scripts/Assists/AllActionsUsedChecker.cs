using System.Collections;
using StormRend.Abilities;
using StormRend.Systems.StateMachines;
using StormRend.UI;
using StormRend.Units;
using UnityEngine;

namespace StormRend.Assists
{
	[RequireComponent(typeof(UnitRegistry))]
	public class AllActionsUsedChecker : MonoBehaviour
	{
		//Inspector
		[SerializeField] float delay = 3f;
		[SerializeField] TurnEndingCountdown endTurnCounter = null;

		//Properties
		UltraStateMachine ultraStateMachine = null;
		UnitRegistry ur = null;

		void Awake()
		{
			ur = GetComponent<UnitRegistry>();
			ultraStateMachine = FindObjectOfType<UltraStateMachine>();
			if (!endTurnCounter) endTurnCounter = FindObjectOfType<TurnEndingCountdown>();

			Debug.Assert(ultraStateMachine, "No Ultra State Machine found!");
			Debug.Assert(endTurnCounter, "No turn end countdown timer passed in!");
		}

		//Register for each unit's onActed events
		void Start()    //OnEnable runs too early
		{
			endTurnCounter.gameObject.SetActive(false);

			foreach (var u in ur.GetAliveUnitsByType<AllyUnit>())
			{
				var au = u as AnimateUnit;
				au.onActed.AddListener(Check);
			}
		}
		void OnDisable()
		{
			foreach (var u in ur.GetAliveUnitsByType<AllyUnit>())
			{
				var au = u as AnimateUnit;
				au.onActed.RemoveListener(Check);
			}
		}

		public void Check(Ability a)
		{
			Debug.Log("Checking if All actions used");
			//If all ally units have used up there actions then automatically end turn
			foreach (var ally in ur.GetAliveUnitsByType<AllyUnit>())
				if (ally.canAct) return;

			//No units can no longer act > all actions used up > end turn
			StartCoroutine(NextTurn(delay));
		}

		IEnumerator NextTurn(float delay)
		{
			//Activate counter
			endTurnCounter?.gameObject.SetActive(true);

			float time = delay;
			while (time > 0)
			{
				time -= Time.deltaTime;
				endTurnCounter?.SetTime(time);
				yield return null;
			}

			ultraStateMachine.NextTurn();

			//Deactivate counter
			endTurnCounter?.gameObject.SetActive(false);
		}
	}
}