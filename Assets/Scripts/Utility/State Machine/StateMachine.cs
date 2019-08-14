using UnityEngine;


/// <summary>
/// state machine which handles the initialisation and transitioning between different states
/// </summary>
public class StateMachine : MonoBehaviour
{
    // state management
    private State m_currentState;
    private State m_previousState;

    #region getters

    public State GetCurrentState() { return m_currentState; }
    public State GetPreviousState() { return m_previousState; }

    #endregion

    /// <summary>
    /// update function which updates the current state
    /// </summary>
    void Update()
    {
        if (m_currentState != null)
            m_currentState.Stay(this);
    }

    /// <summary>
    /// function that handles state initialisation
    /// </summary>
    /// <param name="state">the state to initialise</param>
    public void InitialiseState(State state)
    {
        // update states
        m_previousState = null;
        m_currentState = state;

        // enter new state
        m_currentState.Enter();
    }

    /// <summary>
    /// function that handles state transitions
    /// </summary>
    /// <param name="state">the state to transition to</param>
    public void ChangeState(State state)
    {
        // update states
        m_previousState = m_currentState;
        m_currentState = state;

        // exit previous state
        m_previousState.Exit();

        // enter new state
        m_currentState.Enter();
    }
}