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

		static BhaveManager bhm = BhaveManager.singleton;

		static Unit[] currentUnits = null;

		public void StartTurns()
		{
			//Get all available units of a certain type
			currentUnits = GameManager.GetInstance().GetEnemyUnits();

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
			EndTurnOfUnitType();
		}


		/// <summary>
		/// Ends current unit's turn. If it's the last one then end turn
		/// </summary>
		public void EndTurnOfUnitType()
		{
			if (unitType == UnitType.Enemy)
				GameManager.GetInstance().GetTurnManager().PlayerTurn();
			else if (unitType == UnitType.Player)
				GameManager.GetInstance().GetTurnManager().EnemyTurn();
		}

	}
}
