using System.Collections;
using StormRend.Abilities;
using StormRend.Systems.StateMachines;
using StormRend.Units;
using UnityEngine;

namespace StormRend.Assists 
{ 
	[RequireComponent(typeof(UnitRegistry))]
	public class AllActionsUsedChecker : MonoBehaviour
	{
		//Inspector
		[SerializeField] float delay = 3f;

		//Properties
		UltraStateMachine ultraStateMachine = null;

		UnitRegistry ur = null;
		void Awake()
		{
			//Unit Registry
			ur = GetComponent<UnitRegistry>();

			//Ultra State Machine
			ultraStateMachine = FindObjectOfType<UltraStateMachine>();
			Debug.Assert(ultraStateMachine, "No Ultra State Machine found!");
		}

		//Register for each unit's onActed events
		void Start()	//OnEnable runs too early
		{
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
			yield return new WaitForSeconds(delay);
			ultraStateMachine.NextTurn();
		}
   	}
}