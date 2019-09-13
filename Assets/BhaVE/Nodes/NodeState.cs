namespace BhaVE.Nodes
{
    public enum NodeState
	{
		Aborted = -3,	//Immediately deactivates agent
		Suspended = -2,	//Immediately suspends agent
		None = -1,		//Used in BHEditor
		Failure = 0,
		Success = 1,
		Pending,
	}
}