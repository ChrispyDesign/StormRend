using StormRend.Systems.StateMachines;
using StormRend.Units;
using UnityEngine;

namespace StormRend.States
{
    public sealed class AllyTurnState : TurnState
	{
		UnitRegistry ur;

		AllyUnit[] allies = null;

		void Awake()
		{
			ur = UnitRegistry.current;
		}

		void Start()
		{
			Debug.Assert(ur, "Unit Registry not found!");
		}

		public override void OnEnter(UltraStateMachine sm)
		{
			//NOTE! This must run first because 
			allies = ur.GetAliveUnitsByType<AllyUnit>();
			foreach (var u in allies)
				u.BeginTurn();

			base.OnEnter(sm);
		}

		public override void OnExit(UltraStateMachine sm)
		{
			//Close allies
			allies = ur.GetAliveUnitsByType<AllyUnit>();
			foreach (var u in allies)
				u.EndTurn();

			base.OnExit(sm);
		}

	}
}
