#if UNITY_EDITOR
using UnityEngine;
#endif

namespace BhaVE.Nodes.Leafs
{
	public abstract class Leaf : Node
	{
#if UNITY_EDITOR
	#region Children
		public override bool hasChild => false;
		public override void AddChild(Node child)
		{
			throw new System.InvalidOperationException("Leaf nodes cannot take children!");
		}

		public override void RemoveChild(Node child)
		{
			throw new System.InvalidOperationException("Leaf nodes cannot take children!");
		}

		public override bool ContainsChild(Node child)
		{
			throw new System.InvalidOperationException("Leaf nodes cannot take children!");
		}
	#endregion

	#region BHEditor
		public override void DrawConnections()
		{
			//DO NOTHING as leaf nodes cannot have any children to draw connections to
		}
		public override void SetNodeAndChildren(NodeState state)
		{
			//ONLY FAIL THIS as leaf nodes cannot have any chldren to fail
			this.state = state;
		}
	#endregion
#endif
	}
}
