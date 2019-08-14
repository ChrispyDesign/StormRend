using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class EnemyTurn : State
{
    [SerializeField] private TurnManager m_turnManager;

    // enemy turn timer
    [SerializeField] private float m_enemyTurnTime = 0.5f;
    private float m_timer;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="turnManager"></param>
    public EnemyTurn(TurnManager turnManager)
    {
        m_turnManager = turnManager;
    }

    /// <summary>
    /// 
    /// </summary>
    public override void Enter()
    {
        m_timer = 0;
    }

    /// <summary>
    /// 
    /// </summary>
    public override void Stay(StateMachine stateMachine)
    {
        while (m_timer < m_enemyTurnTime)
        {
            m_timer += Time.deltaTime;
            return;
        }

        m_turnManager.PlayerTurn();
    }

    /// <summary>
    /// 
    /// </summary>
    public override void Exit()
    {
		m_turnManager.ResetPlayerVariables();
    }
}
