﻿using System.Collections.Generic;
using StormRend.Systems;
using UnityEngine;
using UnityEngine.UI;

namespace StormRend.Defunct
{
	/// <summary>
	/// Class responsible for transitioning from player to enemy turn and vice versa. Also responsible for
	/// ensuring turn transitions aren't called inappropriately
	/// </summary>
	[RequireComponent(typeof(ShitStateMachine))]
	public class TurnManager : MonoBehaviour
	{
		//Rename:
		// - Turn controller

		// relevant UI
		[SerializeField] Button m_proceedTurnButton = null;

		int m_currentTurn;

		// state machine for managing turns
		ShitStateMachine m_stateMachine;

		// player and enemy turns
		PlayerTurn m_playerTurn;
		EnemyTurn m_enemyTurn;

		#region getters

		public ShitState GetCurrentTurn() { return m_stateMachine.GetCurrentState(); }
		public ShitState GetPreviousTurn() { return m_stateMachine.GetPreviousState(); }

		#endregion

		/// <summary>
		/// constructor, initialises player and enemy turns
		/// </summary>
		void Start()
		{
			// cache state machine reference
			m_stateMachine = GetComponent<ShitStateMachine>();

			// initialise state machine and player/enemy turns
			m_playerTurn = new PlayerTurn(this);
			m_enemyTurn = new EnemyTurn(this);

			// player turn by default
			m_stateMachine.InitState(m_playerTurn);
		}

		/// <summary>
		/// call this function to begin the player's turn!
		/// </summary>
		public void PlayerTurn()
		{
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
				blizzardManager.Tick();

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
			GameManager.singleton.GetCommandManager().commands.Clear();
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
				player.m_afterClear = false;
			}

			GameManager.singleton.GetCommandManager().ClearCommands();
		}

		public void ResetEnemyVariables()
		{
			EnemyUnit[] units = GameManager.singleton.GetEnemyUnits();

			foreach (EnemyUnit enemy in units)
			{
				enemy.SetHasAttacked(false);
				enemy.SetHasMoved(false);
				enemy.m_afterClear = false;
			}

			GameManager.singleton.GetCommandManager().ClearCommands();
		}
	}
}