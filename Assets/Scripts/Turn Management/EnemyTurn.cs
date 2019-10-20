using System.Collections;
using System.Collections.Generic;
using StormRend;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class EnemyTurn : State
{
    [SerializeField] private TurnManager m_turnManager;

    // enemy turn timer
    [SerializeField] private float m_enemyTurnTime = 1f;
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
    public override void OnEnter()
    {
        m_timer = 0;

        GameManager.singleton.DeselectAllUnits<PlayerUnit>();
    }

    /// <summary>
    /// 
    /// </summary>
    public override void OnUpdate(StateMachine stateMachine)
    {
        //while (m_timer < m_enemyTurnTime)
        //{
        //    m_timer += Time.deltaTime;
        //    return;
        //}

        //m_turnManager.PlayerTurn();
    }

    /// <summary>
    /// 
    /// </summary>
    public override void OnExit()
    {
		m_turnManager.ResetPlayerVariables();
    }
}
