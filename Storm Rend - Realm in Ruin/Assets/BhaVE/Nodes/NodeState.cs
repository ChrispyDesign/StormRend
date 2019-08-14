namespace BhaVE.Nodes
{
    public enum NodeState
	{
		// Aborted = -3,
		// Suspended = -2,
		None = -1,		//Used in BHEditor
		Failure = 0,
		Success = 1,
		Pending,
	}
}