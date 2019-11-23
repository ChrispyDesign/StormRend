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
			base.OnEnter(sm);

			//Prep allies
			allies = ur.GetAliveUnitsByType<AllyUnit>();
			foreach (var u in allies)
				u.BeginTurn();
		}

		public override void OnExit(UltraStateMachine sm)
		{
			base.OnExit(sm);

			//Close allies
			allies = ur.GetAliveUnitsByType<AllyUnit>();
			foreach (var u in allies)
				u.EndTurn();
		}

	}
}
