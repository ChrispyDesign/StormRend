﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class PlayerTurn : State
{
	[SerializeField] private TurnManager m_turnManager;

    // turn timing variables
    private float m_turnTimer = 0;
    private float m_longestTurn = 0;
    private float m_totalTurnTime = 0;

	public PlayerTurn(TurnManager turnManager)
	{
		m_turnManager = turnManager;
	}

    /// <summary>
    /// 
    /// </summary>
    public override void Enter()
    {
        m_turnTimer = 0;
    }

    /// <summary>
    /// 
    /// </summary>
    public override void Stay(StateMachine stateMachine)
    {
        // increment turn timer
        m_turnTimer += Time.deltaTime;
    }

    /// <summary>
    /// 
    /// </summary>
    public override void Exit()
    {
        // update longest turn
        if (m_turnTimer > m_longestTurn)
            m_longestTurn = m_turnTimer;

        // update total turn time
        m_totalTurnTime += m_turnTimer;

		m_turnManager.ResetEnemyVariables();
    }
}