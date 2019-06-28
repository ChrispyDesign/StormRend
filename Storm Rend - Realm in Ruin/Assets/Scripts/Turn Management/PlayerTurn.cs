using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class PlayerTurn : State
{
    // turn timing variables
    private float m_turnTimer = 0;
    private float m_longestTurn = 0;
    private float m_totalTurnTime = 0;

    /// <summary>
    /// 
    /// </summary>
    public override void Enter()
    {
        Debug.Log("Player Turn Start");
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
        Debug.Log("Player Turn End");

        // update longest turn
        if (m_turnTimer > m_longestTurn)
            m_longestTurn = m_turnTimer;

        // update total turn time
        m_totalTurnTime += m_turnTimer;
    }
}