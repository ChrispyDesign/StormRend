﻿using System.Collections;
using pokoro.BhaVE.Core;
using StormRend.Defunct;
using StormRend.Systems.StateMachines;
using UnityEngine;

namespace StormRend.States
{
	public sealed class EnemyTurnState : TurnState
	{
		// - Tick each enemy's AI in sequence
		// - Trigger crystals
		// - Handle any UI

		// public enum EnemyType
		// 	{ FrostTroll, FrostHound }
		// [SerializeField] EnemyType tbaEnemyType;

		[Tooltip("Time between each enemy unit's turn in seconds")]
		[SerializeField] float aiTurnTime = 2f;

		xUnit[] currentEnemies;
		BhaveDirector ai;
		xGameManager gm;

		void Awake()
		{
			ai = BhaveDirector.singleton;
			gm = xGameManager.singleton;
		}

		public override void OnEnter(UltraStateMachine sm)
		{
			base.OnEnter(sm);

			//Get the current enemies & Run AI
			currentEnemies = gm.GetEnemyUnits();
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

			//Tick crystals
			TickCrystals();

			//Finish enemy turn
			sm.NextTurn();
		}

		void TickCrystals()
		{
			//Tick crystals
			foreach (var c in xGameManager.singleton.GetCrystals())
			{
				c.IterateTurns();
			}
		}
	}
}