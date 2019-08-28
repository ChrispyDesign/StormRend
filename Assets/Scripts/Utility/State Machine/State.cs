/// <summary>
/// abstract base state class. Contains pure virtual functions for entering, updating and exiting states
/// </summary>
public abstract class State
{
    public abstract void OnEnter();
    public abstract void OnUpdate(StateMachine stateMachine);
    public abstract void OnExit();
}