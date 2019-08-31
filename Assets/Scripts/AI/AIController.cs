using System.Collections;
using BhaVE.Core;
using UnityEngine;

//TODO temp
using StormRend.Defunct;
using StormRend.States;
using StormRend.Systems.StateMachines;

namespace StormRend.AI
{
    public class AIController : MonoBehaviour
	{
		// - Is triggered by player finish turn event
		// - Runs each enemy's AI one by one
		// - Adjustable delay between each enemy's turn
		// - API to end enemy's turn accessible by delegates

		public enum UnitType
			{ Player, Enemy }

		[SerializeField] UnitType aiUnitType;
		[SerializeField] float aiTurnTime = 2f;  //Seconds

		Unit[] currentUnits = null;

        UltraStateMachine tbssm;
		BhaveDirector bd = null;

		//Death row below
        GameManager gm = null;

		void Awake()
		{
			tbssm = GameStateDirector.singleton as UltraStateMachine;
			bd = BhaveDirector.singleton;

			gm = GameManager.singleton;
		}

		public void StartAITurn()
		{
			//Get all CURRENT available units of a certain type
			if (aiUnitType == UnitType.Player)
				currentUnits = gm.GetPlayerUnits();
			else if (aiUnitType == UnitType.Enemy)
				currentUnits = gm.GetEnemyUnits();

			//Start running the AI
			StartCoroutine(RunAI());
		}

		IEnumerator RunAI()
		{
			//Run through each unit's turn
			foreach (var u in currentUnits)
			{
				var agent = u.GetComponent<BhaveAgent>();
				bd.Tick(agent);
				yield return new WaitForSeconds(aiTurnTime);
			}

			//Then end the turn for this unit type
			EndAITurn();
		}

		/// <summary>
		/// Ends current unit's turn. If it's the last one then end turn
		/// </summary>
		public void EndAITurn()
		{
			//This shit needs to be something like GameStateDirector.NextTurn()
			tbssm.NextTurn();

			// if (aiUnitType == UnitType.Enemy)
			// 	gm.GetTurnManager().PlayerTurn();
			// else if (aiUnitType == UnitType.Player)
			// 	gm.GetTurnManager().EnemyTurn();
		}
	}
}
