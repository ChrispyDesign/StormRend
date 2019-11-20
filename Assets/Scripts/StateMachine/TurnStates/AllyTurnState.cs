using StormRend.Systems.StateMachines;
using StormRend.Units;
using UnityEngine;

namespace StormRend.States
{
    public sealed class AllyTurnState : TurnState
	{
		UnitRegistry ur;

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

			//Calculate possible moves
			ur.RunUnitsBeginTurn(this);

		}

	}
}
