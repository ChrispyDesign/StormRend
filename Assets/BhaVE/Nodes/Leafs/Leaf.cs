using UnityEngine;

namespace BhaVE.Nodes.Leafs
{
	public abstract class Leaf : Node
	{
		public override bool hasChild => false;

#if UNITY_EDITOR
		public override void DrawConnections()
		{
			//DO NOTHING as leaf nodes cannot have any children to draw connections to
		}

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

		public override void FailNodeAndChildren()
		{
			//ONLY FAIL THIS as leaf nodes cannot have any chldren to fail
			this.state = NodeState.Failure;
		}
#endif
	}
}
