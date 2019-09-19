using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StormRend
{
	/// <summary>
	/// Class responsible for transitioning from player to enemy turn and vice versa. Also responsible for
	/// ensuring turn transitions aren't called inappropriately
	/// </summary>
	[RequireComponent(typeof(StateMachine))]
	public class TurnManager : MonoBehaviour
	{
		//Rename:
		// - Turn controller

		// relevant UI
		[SerializeField] Button m_proceedTurnButton = null;

		int m_currentTurn;
		GameObject m_playerFlag;
		GameObject m_enemyFlag;

		// state machine for managing turns
		StateMachine m_stateMachine;

		// player and enemy turns
		PlayerTurn m_playerTurn;
		EnemyTurn m_enemyTurn;

		#region getters

		public State GetCurrentTurn() { return m_stateMachine.GetCurrentState(); }
		public State GetPreviousTurn() { return m_stateMachine.GetPreviousState(); }

		#endregion

		/// <summary>
		/// constructor, initialises player and enemy turns
		/// </summary>
		void Start()
		{
			// cache state machine reference
			m_stateMachine = GetComponent<StateMachine>();

			// initialise state machine and player/enemy turns
			m_playerTurn = new PlayerTurn(this);
			m_enemyTurn = new EnemyTurn(this);

			m_playerFlag = UIManager.GetInstance().GetPlayerFlag();
			m_enemyFlag = UIManager.GetInstance().GetEnemyFlag();
			m_playerFlag.SetActive(true);
			m_enemyFlag.SetActive(false);

			// player turn by default
			m_stateMachine.InitState(m_playerTurn);
		}

		/// <summary>
		/// call this function to begin the player's turn!
		/// </summary>
		public void PlayerTurn()
		{
			m_enemyFlag.SetActive(false);
			m_playerFlag.SetActive(true);
			List<Crystal> crystal = GameManager.singleton.GetCrystals();
			foreach (Crystal c in crystal)
			{
				c.IterateTurns();
			}
			m_currentTurn++;

			// ensure blizzard manager exists
			BlizzardController blizzardManager = UIManager.GetInstance().GetBlizzardManager();

			// increment blizzard counter at the start of each turn
			if (blizzardManager)
				blizzardManager.IncrementBlizzardMeter();

			// enable proceed button
			m_proceedTurnButton.interactable = true;

			// proceed to player turn
			m_stateMachine.Switch(m_playerTurn);
		}

		/// <summary>
		/// call this function to begin the enemy's turn!
		/// </summary>
		public void EnemyTurn()
		{
			m_playerFlag.SetActive(false);
			m_enemyFlag.SetActive(true);
			GameManager.singleton.GetCommandManager().m_moves.Clear();
			foreach(PlayerUnit player in GameManager.singleton.GetPlayerUnits())
			{
				player.SetMoveCommand(null);
			}

			// disable proceed button
			m_proceedTurnButton.interactable = false;

			// proceed to enemy turn
			m_stateMachine.Switch(m_enemyTurn);
		}

		public void ResetPlayerVariables()
		{
			PlayerUnit[] units = GameManager.singleton.GetPlayerUnits();

			foreach (PlayerUnit player in units)
			{
				player.SetHasAttacked(false);
				player.SetHasMoved(false);
				player.isChained = false;
				player.isBlind = false;
				player.isProtected = false;
				player.isCrippled = false;
				player.isProvoking = false;
			}

			EnemyUnit[] enemyUnits = GameManager.singleton.GetEnemyUnits();

			foreach (EnemyUnit enemy in enemyUnits)
			{
				enemy.isBlind = false;
				enemy.isProtected = false;
				enemy.isCrippled = false;
				enemy.isProvoking = false;
			}

			GameManager.singleton.GetCommandManager().m_moves.Clear();
		}

		public void ResetEnemyVariables()
		{
			EnemyUnit[] units = GameManager.singleton.GetEnemyUnits();

			foreach (EnemyUnit enemy in units)
			{
				enemy.SetHasAttacked(false);
				enemy.SetHasMoved(false);
				enemy.isChained = false;
			}

			GameManager.singleton.GetCommandManager().m_moves.Clear();
		}
	}
}