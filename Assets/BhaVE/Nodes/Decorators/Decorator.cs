using UnityEngine;
using BhaVE.Core;
#if UNITY_EDITOR
using BhaVE.Editor;
#endif

namespace BhaVE.Nodes.Decorators
{
	public abstract class Decorator : Node
	{
		[SerializeField] internal Node child = null;

		#region Core
		//Pass down methods to the child
		protected internal override void OnAwaken(BhaveAgent agent) => child?.OnAwaken(agent);
		protected internal override void OnInitiate(BhaveAgent agent) => child?.OnInitiate(agent);
		protected internal override void OnBegin() => child?.OnBegin();
		protected internal override void OnPause(bool paused) => child?.OnPause(paused);
		protected internal override void OnEnd() => child?.OnEnd();
		protected internal override void OnShutdown() => child?.OnShutdown();
		#endregion

#if UNITY_EDITOR
		protected override void OnDestroy()
		{
			//Remove relationships from parent
			base.OnDestroy();

			//Make sure child's parent reference is cleared
			if (hasChild)
			{
				Debug.LogFormat("Child:[{0}:{1}] disowns its Parent",
					child.GetType().Name, child.ID);
				child.parent = null;
			}
		}

		#region Children
		public override bool hasChild => child != null;
		public override void AddChild(Node n)
		{
			try
			{
				if (!n) throw new System.ArgumentNullException("Node to chlid is null!");

				if (n == parent) throw new System.InvalidOperationException("Cannot child parent!");

				//If the new node has a parent, disconnect from each other
				n.parent?.RemoveChild(n);

				//If this node has a child then disconnect as decorators can ONLY have one child
				if (hasChild)
				{
					Debug.LogFormat("Nulling [{0}:{1}] parent",
						child.GetType().Name, child.ID);
					child.parent = null;
				}

				//Child the new node then set it's parent to this
				child = n;
				n.parent = this;
			}
			catch (System.Exception e)
			{
				Debug.LogWarning(e.Message);
			}
		}

		/// <summary>
		/// Disconnects and removes child
		/// </summary>
		public override void RemoveChild(Node xxx = null)
		{
			//Ignore whatever's passed in
			xxx = null;

			try
			{
				//If this node doesn't have a child
				if (!child)
				{
					throw new System.NullReferenceException(string.Format("[{0}:{1}] does not have a child!",
						this.GetType().Name, this.ID));
				}
				else
				{
					//Double check that the specified node to be deleted actually belongs to us
					//Because by definition, if it is this node's child, then it's parent should be this node
					if (child.parent == this)
					{
						Debug.LogFormat("[{0}:{1}] and [{2}:{3}] disowning from each other",
							child.GetType().Name, child.ID, this.GetType().Name, this.ID);

						//Disconnect child's parent (ie. this)
						child.parent = null;

						//Disconnect the child
						child = null;
					}
					else
					{
						throw new System.ArgumentException(string.Format("[{0}:{1}] is not a child of [{2}:{3}]!",
							child.GetType().Name, child.ID, this.GetType().Name, child.ID));
					}
				}
			}
			catch (System.Exception e)
			{
				Debug.LogWarning(e.Message);
			}
		}

		public override bool ContainsChild(Node n)
		{
			return child == n;
		}

		public override void SetNodeAndChildren(NodeState state)
		{
			this.state = state;
			child = null;
		}
		#endregion

		public override void DrawConnections()
		{
			//Draw connection to child if child exists
			if (child) BhaVEditor.DrawConnection(this, child);
		}
#endif
	}
}