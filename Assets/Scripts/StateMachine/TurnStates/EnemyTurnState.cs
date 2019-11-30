using System.Collections;
using System.Collections.Generic;
using pokoro.BhaVE.Core;
using StormRend.Systems.StateMachines;
using StormRend.Units;
using UnityEngine;

namespace StormRend.States
{
    public sealed class EnemyTurnState : TurnState
	{
		// - Tick each enemy's AI in sequence
		// - Trigger crystals ! Maybe use UnityEvents and decouple this to another MonoBehaviour
		// - Handle any UI : Use UnityEvents

		[Header("AI"), Tooltip("Time between each enemy unit's turn in seconds")]
		[SerializeField] float AITurnTime = 2f;

		HashSet<EnemyUnit> enemies = new HashSet<EnemyUnit>();
		BhaveDirector ai;
		UnitRegistry ur;

		void Awake()
		{
			ai = BhaveDirector.current;
			ur = UnitRegistry.current;
		}
		void Start()
		{
			Debug.Assert(ai, "BhaVE director not found!");
			Debug.Assert(ur, "Unit Registry not found!");
		}

		public override void OnEnter(UltraStateMachine sm)
		{
			StopAllCoroutines();

			base.OnEnter(sm);

			/* BUG REPORT: AI logic occasionally runs twice in a row
			Preventative measures:
			- Clear the collection of enemies each turn. There might have been lingering duplicates from the last turn
			- Use a hashset to further prevent duplicates
			*/
			enemies.Clear();
			foreach (var enemy in ur.GetAliveUnitsByType<EnemyUnit>())
				enemies.Add(enemy);	
			if (enemies.Count > 0) StartCoroutine(EnemySequence(sm));
		}

		IEnumerator EnemySequence(UltraStateMachine sm)
		{
			//Run through each unit's turn then finish turn
			foreach (var u in enemies)
			{
				var agent = u.GetComponent<BhaveAgent>();
				ai.Tick(agent);
				yield return new WaitForSeconds(AITurnTime);
			}

			//Tick crystals HARDCODE
			TickCrystals();

			//Finish enemy turn
			sm.NextTurn();
		}

		void TickCrystals()
		{
			//Get current crystals
			var crystals = ur.GetAliveUnitsByType<CrystalUnit>();

			//Tick crystals
			foreach (var c in crystals)
				c.Tick();
		}
	}
}
