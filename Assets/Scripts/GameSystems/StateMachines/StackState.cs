namespace StormRend.Systems.StateMachines
{
    public abstract class StackState : State
    {
        internal virtual void OnCover() {}

        internal virtual void OnUncover() {}
    }
}
