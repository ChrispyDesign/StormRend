using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class responsible for transitioning from player to enemy turn and vice versa. Also responsible for
/// ensuring turn transitions aren't called innappropriately
/// </summary>
[RequireComponent(typeof(StateMachine))]
public class TurnManager : MonoBehaviour
{
    // relevant UI
    [SerializeField] private Button m_proceedTurnButton;

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
        m_playerTurn = new PlayerTurn();
        m_enemyTurn = new EnemyTurn(this);
        
        // player turn by default
        m_stateMachine.InitialiseState(m_playerTurn);
    }

    /// <summary>
    /// call this function to begin the player's turn!
    /// </summary>
    public void PlayerTurn()
    {
        // enable proceed button
        m_proceedTurnButton.interactable = true;

        // proceed to player turn
        m_stateMachine.ChangeState(m_playerTurn);
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
}