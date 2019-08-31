using System;
using StormRend.AI;
using StormRend.Systems.StateMachines;
using UnityEngine;

namespace StormRend.States
{
	public sealed class EnemyTurnState : TurnState
	{
		public enum EnemyType
			{ FrostTroll, FrostHound }
		[SerializeField] EnemyType enemyType;
		[SerializeField] AIController aiController;

		void Awake()
		{
			aiController = FindObjectOfType<AIController>();
		}

		public override void OnEnter(UltraStateMachine sm)
		{
			base.OnEnter(sm);

			//Run AI stuff
			aiController.StartAITurn();
		}

		/* Brainstorm
		Enter
		  - StartAITurn
		Update
		  -
		Exit
		*/
	}
}
