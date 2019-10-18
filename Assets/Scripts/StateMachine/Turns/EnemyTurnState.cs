using System.Collections;
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

		[Tooltip("Time between each enemy unit's turn in seconds")]
		[SerializeField] float aiTurnTime = 2f;

		Unit[] currentEnemies;
		BhaveDirector ai;
		UnitRegistry ur;	//Kinda bad

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
			base.OnEnter(sm);

			//Get the current enemies & Run AI
			currentEnemies = ur.GetUnits<EnemyUnit>();
			StartCoroutine(RunAI(sm));
		}

		IEnumerator RunAI(UltraStateMachine sm)
		{
			//Run through each unit's turn then finish turn
			foreach (var u in currentEnemies)
			{
				var agent = u.GetComponent<BhaveAgent>();
				ai.Tick(agent);
				yield return new WaitForSeconds(aiTurnTime);
			}

			//Tick crystals (kinda bad)
			TickCrystals();

			//Finish enemy turn
			sm.NextTurn();
		}

		void TickCrystals()
		{
			//Get current crystals
			var crystals = ur.GetUnits<CrystalUnit>();

			//Tick crystals
			foreach (var c in crystals)
				c.Tick();
		}
	}
}
