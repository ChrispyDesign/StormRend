using StormRend.Enums;
using StormRend.States;
using StormRend.Systems.StateMachines;
using StormRend.Units;
using UnityEngine;

namespace StormRend.Assists 
{ 
	/// <summary>
	/// Runs start turns methods of units based on the state passed in
	/// </summary>
	public class UnitTurnStarter : MonoBehaviour
	{
		UnitRegistry ur;

		void Awake() => ur = UnitRegistry.current;

		public void RunStartTurns(State state)
		{
			var turnstate = state as TurnState;
			AnimateUnit[] currentStateUnits = null;
			switch (turnstate.unitType)
			{
				case TargetType.Allies:
					currentStateUnits = ur.GetAliveUnitsByType<AllyUnit>();
					break;
				case TargetType.Enemies:
					currentStateUnits = ur.GetAliveUnitsByType<EnemyUnit>();
					break;
			}

			foreach (var au in currentStateUnits)
				au.StartTurn();
		}
   	}
}