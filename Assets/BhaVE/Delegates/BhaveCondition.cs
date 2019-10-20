using System;
using BhaVE.Nodes;

namespace BhaVE.Delegates
{
	[Serializable]
	public abstract class BhaveCondition : BhaveDelegate<NodeState> {}

	#region Experimental
	public abstract class BhaveCondition<T1> : BhaveDelegate<T1, NodeState> { }
	public abstract class BhaveCondition<T1, T2> : BhaveDelegate<T1, T2, NodeState> { }
	public abstract class BhaveCondition<T1, T2, T3> : BhaveDelegate<T1, T2, T3, NodeState> { }

	#endregion
}