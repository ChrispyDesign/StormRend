using System;
using BhaVE.Nodes;

namespace BhaVE.Delegates
{
	//NOTE: Actions shouldn't generally return Failure
	//ie. if a SeekAction as quite reached it's destination then return Pending NOT Failure
	
	[Serializable]
	public abstract class BhaveAction : BhaveDelegate<NodeState> { }

	#region Experimental
	public abstract class BhaveAction<T1> : BhaveDelegate<T1, NodeState> { }
	public abstract class BhaveAction<T1, T2> : BhaveDelegate<T1, T2, NodeState> { }
	public abstract class BhaveAction<T1, T2, T3> : BhaveDelegate<T1, T2, T3, NodeState> { }
	#endregion
}