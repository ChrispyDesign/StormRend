/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

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