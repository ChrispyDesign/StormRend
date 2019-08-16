namespace BhaVE.Nodes
{
    public enum NodeState
	{
		Aborted = -3,	//Used to Deactivate agent + EHEditor
		Suspended = -2,	//Used to Pause agent + BHEditor
		None = -1,		//Used in BHEditor
		Failure = 0,
		Success = 1,
		Pending = 2,
	}
}