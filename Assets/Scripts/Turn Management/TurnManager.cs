using System.Collections.Generic;
using StormRend;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class responsible for transitioning from player to enemy turn and vice versa. Also responsible for
/// ensuring turn transitions aren't called inappropriately
/// </summary>
[RequireComponent(typeof(StateMachine))]
public class TurnManager : MonoBehaviour
{
    // relevant UI
    [SerializeField] private Button m_proceedTurnButton = null;

	private int m_currentTurn;

    // state machine for managing turns
    private StateMachine m_stateMachine;

    // player and enemy turns
    private PlayerTurn m_playerTurn;
    private EnemyTurn m_enemyTurn;

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
        
        // player turn by default
        m_stateMachine.InitialiseState(m_playerTurn);
    }

    /// <summary>
    /// call this function to begin the player's turn!
    /// </summary>
    public void PlayerTurn()
    {
        // ensure blizzard manager exists
        BlizzardManager blizzardManager = UIManager.GetInstance().GetBlizzardManager();

        // increment blizzard counter at the start of each turn
        if (blizzardManager)
            blizzardManager.IncrementBlizzardMeter();

        // enable proceed button
        m_proceedTurnButton.interactable = true;

        // proceed to player turn
        m_stateMachine.ChangeState(m_playerTurn);

		List<Crystal> crystal = GameManager.GetInstance().GetCrystals();
		foreach(Crystal c in crystal)
		{
			c.IterateTurns();
		}
		m_currentTurn++;
	}

    /// <summary>
    /// call this function to begin the enemy's turn!
    /// </summary>
    public void EnemyTurn()
    {
        // disable proceed button
        m_proceedTurnButton.interactable = false;

        // proceed to enemy turn
        m_stateMachine.ChangeState(m_enemyTurn);
    }

	public void ResetPlayerVariables()
	{
		PlayerUnit[] units = GameManager.GetInstance().GetPlayerUnits();

		foreach(PlayerUnit player in units)
		{
			player.SetAlreadyAttacked(false);
			player.SetAlreadyMoved(false);
			player.m_afterClear = false;
		}

		GameManager.GetInstance().GetCommandManager().m_moves.Clear();
	}

	public void ResetEnemyVariables()
	{
		EnemyUnit[] units = GameManager.GetInstance().GetEnemyUnits();

		foreach (EnemyUnit enemy in units)
		{
			enemy.SetAlreadyAttacked(false);
			enemy.SetAlreadyMoved(false);
			enemy.m_afterClear = false;
		}

		GameManager.GetInstance().GetCommandManager().m_moves.Clear();
	}
}