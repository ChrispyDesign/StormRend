using System.Collections.Generic;
using StormRend.Defunct;
using StormRend.Systems.StateMachines;

namespace StormRend.States
{
	public sealed class CrystalTurnState : State
	{
		//Crystal registry class?

		GameManager gm;
		List<Crystal> crystals;

		void Awake()
		{
			gm = GameManager.singleton;
		}

		public override void OnEnter(UltraStateMachine sm)
		{
			//Tick all crystals
			crystals = gm.GetCrystals();
			foreach (var c in crystals)
				c.IterateTurns();

			//Next turn
			sm.NextTurn();
		}
	}
}