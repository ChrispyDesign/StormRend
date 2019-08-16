using System.Collections;
using System.Collections.Generic;
using BhaVE.Core;
using UnityEngine;

namespace StormRend.AI
{
	public class AIController : MonoBehaviour
	{
		// - Is triggered by player finish turn event
		// - Runs each enemy's AI one by one
		// - Adjustable delay between each enemy's turn
		// - API to end enemy's turn accessible by delegates

		public enum UnitType { Player, Enemy }
		[SerializeField] UnitType unitType;
		[SerializeField] float delayBetweenTurns = 3f;  //Seconds

		Unit[] currentUnits = null;
		GameManager gm = null;
		BhaveManager bhm = null;

		void Awake()
		{
			gm = GameManager.singleton;
			bhm = BhaveManager.singleton;
		}

		public void StartAITurn()
		{
			//Get all available units of a certain type
			if (unitType == UnitType.Player)
				currentUnits = gm.GetPlayerUnits();
			else if (unitType == UnitType.Enemy)
				currentUnits = gm.GetEnemyUnits();

			//Start running the AI
			StartCoroutine(RunAI());
		}

		IEnumerator RunAI()
		{
			//Run through each unit's turn
			foreach (var u in currentUnits)
			{
				bhm.Tick(u.GetComponent<BhaveAgent>());
				yield return new WaitForSeconds(delayBetweenTurns);
			}

			//Then end the turn for this unit type
			EndAITurn();
		}

		/// <summary>
		/// Ends current unit's turn. If it's the last one then end turn
		/// </summary>
		public void EndAITurn()
		{
			if (unitType == UnitType.Enemy)
				gm.GetTurnManager().PlayerTurn();
			else if (unitType == UnitType.Player)
				gm.GetTurnManager().EnemyTurn();
		}

	}
}
