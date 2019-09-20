using System.Collections.Generic;
using UnityEngine;
using BhaVE.Core;
#if UNITY_EDITOR
using System;
using BhaVE.Editor;
#endif

namespace BhaVE.Nodes.Composites
{
	public abstract class Composite : Node
	{
		[SerializeField] internal List<Node> children = new List<Node>();
#region Children
#if UNITY_EDITOR
		public override bool hasChild => children.Count > 0;

        /// <summary>Connects n to this node while removing any parent connections where necessary</summary>
        /// <param name="n">The node to connect</param>
        public override void AddChild(Node n)
		{
			try
			{
				if (!n) throw new ArgumentNullException("Node to child is null!");

				if (n == parent) throw new System.InvalidOperationException("Cannot child parent!");

				//If the node has a parent then disconnect itself from it's parent
				n.parent?.RemoveChild(n);

				//Add child and then set it's parent to this
				children.Add(n);
				n.parent = this;
			}
			catch (System.Exception e)
			{
				Debug.LogWarning(e.Message);
			}
		}

		/// <summary> Disconnects and removes n from this node </summary>
		/// <param name="n">The node to disconnect</param>
		public override void RemoveChild(Node n)
		{
			try
			{
				if (!n)
				{
					throw new ArgumentNullException("Specified node to disown is null!");
				}
				else
				{
					//Double check that the specified node to be deleted actually belongs to us
					//Because by definition, if it is this node's child, then it's parent should be this node
					if (n.parent == this)
					{
						//Disconnect child's parent (ie. this)
						n.parent = null;

						//Disconnect the child
						children.Remove(n);

						Debug.LogFormat("[{0}:{1}] and [{2}:{3}] disowning each other",
							n.GetType().Name, n.ID, this.GetType().Name, this.ID);
					}
					else
						throw new ArgumentException(string.Format("[{0}:{1}] is not a child of [{2}:{3}]",
							n.GetType().Name, n.ID, this.GetType().Name, this.ID));
				}
			}
			catch (Exception e)
			{
				Debug.LogWarning(e.Message);
			}
		}

		/// <summary> Is this node connected to n? </summary>
		/// <param name="n">The node to check</param>
		public override bool ContainsChild(Node n)
		{
			return children.Contains(n);
		}
#endif
#endregion

#region Core
        //Pass down methods to the children
        protected internal override void OnAwaken(BhaveAgent agent)
		{
			foreach (var c in children)
				c.OnAwaken(agent);
		}
        protected internal override void OnInitiate(BhaveAgent agent)
		{
			foreach (var c in children)
				c.OnInitiate(agent);
		}
		protected internal override void OnBegin()
		{
			foreach (var c in children)
				c.OnBegin();
		}
		protected internal override void OnEnd()
		{
			foreach (var c in children)
				c.OnEnd();
		}
		protected internal override void OnShutdown()
		{
			foreach (var c in children)
				c.OnShutdown();
		}
		protected internal override void OnPause(bool paused)
		{
			foreach (var c in children)
				c.OnPause(paused);
		}
#endregion

#if UNITY_EDITOR
		protected override void OnDestroy()
		{
			//Remove relationship from parent
			base.OnDestroy();

			//Remove relationships from children
			foreach (var c in children)
			{
				Debug.LogFormat("Child:[{0}:{1}] disowns Parent",
					c.GetType().Name, c.ID);
				c.parent = null;
			}
		}

		public override void DrawConnections()
		{
			//Draw connections for all of this node's active children
			foreach (var c in children)
			{
				if (c) BhaVEditor.DrawConnection(this, c);
			}
		}

		public override void SetNodeAndChildren(NodeState state)
		{
			this.state = state;
			foreach (Node c in children)
			{
				c.SetNodeAndChildren(state);
			}
		}
#endif
	}
}

#region Behaviour Designer API Ideas
// The maximum number of children a parent task can have. Will usually be 1 or int.MaxValue
// public virtual int MaxChildren();
// // Boolean value to determine if the current task is a parallel task.
// public virtual bool CanRunParallelChildren();
// // The index of the currently active child.
// public virtual int CurrentChildIndex();
// // Boolean value to determine if the current task can execute.
// public virtual bool CanExecute();
// // Apply a decorator to the executed status.
// public virtual NodeState Decorate(NodeState status);
// // Notifies the parent task that the child has been executed and has a status of childStatus.
// public virtual void OnChildExecuted(NodeState childStatus);
// // Notifies the parent task that the child at index childIndex has been executed and has a status of childStatus.
// public virtual void OnChildExecuted(int childIndex, NodeState childStatus);
// // Notifies the task that the child has started to run.
// public virtual void OnChildStarted();
// // Notifies the parallel task that the child at index childIndex has started to run.
// public virtual void OnChildStarted(int childIndex);
// // Some parent tasks need to be able to override the status, such as parallel tasks.
// public virtual NodeState OverrideStatus(NodeState status);
// // The interrupt node will override the status if it has been interrupted.
// public virtual NodeState OverrideStatus();
// // Notifies the composite task that an conditional abort has been triggered and the child index should reset.
// public virtual void OnConditionalAbort(int childIndex);
#endregion