using StormRend;
using UnityEngine;


/// <summary>
/// state machine which handles the initialisation and transitioning between different states
/// </summary>
public class StateMachine : MonoBehaviour
{
    //Improve this

    // state management
    private ShitState m_currentState;
    private ShitState m_previousState;      //Why is this needed?

    #region getters

    public ShitState GetCurrentState() { return m_currentState; }
    public ShitState GetPreviousState() { return m_previousState; }

    #endregion

    /// <summary>
    /// update function which updates the current state
    /// </summary>
    void Update()
    {
        if (m_currentState != null)
            m_currentState.OnUpdate(this);
    }

    /// <summary>
    /// function that handles state initialisation
    /// </summary>
    /// <param name="state">the state to initialise</param>
    public void InitState(ShitState state)
    {
        // update states
        m_previousState = null;
        m_currentState = state;

        // enter new state
        m_currentState.OnEnter();
    }

    /// <summary>
    /// function that handles state transitions
    /// </summary>
    /// <param name="state">the state to transition to</param>
    public void Switch(ShitState state)
    {
        // update states
        m_previousState = m_currentState;
        m_currentState = state;

        // exit previous state
        m_previousState.OnExit();

        // enter new state
        m_currentState.OnEnter();
    }
}