using StormRend.States;
using StormRend.Systems;
using StormRend.Systems.StateMachines;
using UnityEngine;

namespace StormRend.MapSystems.Tiles 
{ 
	public class CrumblingTile : Tile
	{
		// public enum TurnTypeAffect { AllysTurn, EnemyTurns }

		//Inspector
		[SerializeField] int turnsBeforeCrumbling = 2;

		//Events


		//Members
		static bool newTurn = false;
		int turnCount = 0;
		GameDirector gd = null;
		UltraStateMachine usm = null;

		void Awake()
		{
			newTurn = true;
			turnCount = 0;
			gd = GameDirector.current;
			usm = gd.GetComponent<UltraStateMachine>();
		}

		void OnEnable() => usm.onExitCurrentTurn.AddListener(OnTurnExit);
		void OnDisable() => usm.onExitCurrentTurn.RemoveListener(OnTurnExit);

		void OnTurnExit(State state)
		{
			//Reset flags
			newTurn = false;

			var allyTurnState = state as AllyTurnState;
			if (allyTurnState)
			{

			}
		}
   	}
}