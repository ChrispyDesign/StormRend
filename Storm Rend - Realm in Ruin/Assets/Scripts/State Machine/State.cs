/// <summary>
/// abstract base state class. Contains pure virtual functions for entering, updating and exiting states
/// </summary>
public abstract class State
{
    public abstract void Enter();
    public abstract void Stay(StateMachine stateMachine);
    public abstract void Exit();
}