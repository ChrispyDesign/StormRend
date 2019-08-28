using System.Collections;
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
		[SerializeField] UnitType aiUnitType;
		[SerializeField] float delayBetweenNextUnitTurn = 2f;  //Seconds

		Unit[] currentUnits = null;
		GameManager gm = null;
		BhaveDirector bhd = null;


		void Awake()
		{
			gm = GameManager.singleton;
			bhd = BhaveDirector.singleton;
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
				bhd.Tick(agent);
				yield return new WaitForSeconds(delayBetweenNextUnitTurn);
			}

			//Then end the turn for this unit type
			EndAITurn();
		}

		/// <summary>
		/// Ends current unit's turn. If it's the last one then end turn
		/// </summary>
		public void EndAITurn()
		{
			if (aiUnitType == UnitType.Enemy)
				gm.GetTurnManager().PlayerTurn();
			else if (aiUnitType == UnitType.Player)
				gm.GetTurnManager().EnemyTurn();
		}
	}
}
