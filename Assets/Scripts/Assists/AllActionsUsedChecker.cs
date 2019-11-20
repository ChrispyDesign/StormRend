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
		[SerializeField] EndTurnCountdown endTurnCountdown = null;

		//Properties
		UltraStateMachine ultraStateMachine = null;
		UnitRegistry ur = null;

		void Awake()
		{
			ur = GetComponent<UnitRegistry>();
			ultraStateMachine = FindObjectOfType<UltraStateMachine>();
			if (!endTurnCountdown) endTurnCountdown = FindObjectOfType<EndTurnCountdown>();

			Debug.Assert(ultraStateMachine, "No Ultra State Machine found!");
			Debug.Assert(endTurnCountdown, "No turn end countdown timer passed in!");
		}

		//Register for each unit's onActed events
		void Start()    //OnEnable runs too early
		{
			endTurnCountdown.gameObject.SetActive(false);

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
			Debug.Log("Check All Actions Used");
			//If all ally units have used up there actions then automatically end turn
			foreach (var ally in ur.GetAliveUnitsByType<AllyUnit>())
				if (ally.canAct) return;

			//No units can no longer act > all actions used up > end turn
			StartCoroutine(NextTurn(delay));
		}

		IEnumerator NextTurn(float delay)
		{
			//Activate counter
			endTurnCountdown?.gameObject.SetActive(true);

			float time = delay;
			while (time > 0)
			{
				time -= Time.deltaTime;
				endTurnCountdown?.SetTime(time);
				yield return null;
			}

			ultraStateMachine.NextTurn();

			//Deactivate counter
			endTurnCountdown?.gameObject.SetActive(false);
		}
	}
}