namespace StormRend.Defunct
{
    /// <summary>
    /// abstract base state class. Contains pure virtual functions for entering, updating and exiting states
    /// </summary>
    public abstract class ShitState
    {
        public abstract void OnEnter();
        public abstract void OnUpdate(ShitStateMachine stateMachine);
        public abstract void OnExit();
    }
}