using UnityEngine;
using BhaVE.Core;
using BhaVE.Editor;
using BhaVE.Editor.Nodes;
using BhaVE.Editor.Data;
using UnityEditor;

namespace BhaVE.Nodes
{
    [System.Serializable]
	public abstract class Node : ScriptableObject, IBHNode, IBHENode
	{
#if UNITY_EDITOR
		[SerializeField] internal BHEData eData = new BHEData();
#endif
		public int ID;

		//State
		// - State of this node can be queried by the public if need by (maybe in realtime in editor)
		// - Should be set in Evaluate()
		[SerializeField] protected NodeState _state = NodeState.None;
		public NodeState state
		{
			get => _state;
			internal set => _state = value;
		}

		//Parent
		// - All nodes can have ONE parent only. 
		// - These should be set automatically during connecting and disconnecting
		[SerializeField] internal Node parent;
		public bool hasParent => parent != null;

		//Children
		// - This need to be implemented by Composite and Decorators
		// - All other nodes will have a null implementation
		public abstract bool hasChild { get; }
		public abstract void AddChild(Node n);
		public abstract void RemoveChild(Node n);
		public abstract bool ContainsChild(Node n);

		#region Core Methods
		// - These should be implemented by Action and Condition nodes
		// - Accessible by: Inherited nodes, Editor
		// - The virtual methods needs to run its child node(s) where necessary

		/// <summary> Called once upon start up. Use to construct. </summary>
		protected internal virtual void OnInitiate(BhaveAgent agent) { }

		/// <summary> Called once each tick cycle BEFORE node evaluation </summary>
		protected internal virtual void OnBegin() { }

		/// <summary> Evaluates the node returning a node state value. Call frequency according to system tick interval settings </summary>
		protected internal abstract NodeState OnExecute(BhaveAgent agent);

		/// <summary> Called according to tick interval settings during BhaVE system pause </summary>
		protected internal virtual void OnPause(bool paused) { }

		/// <summary> Called once each tick cycle AFTER node evaluation </summary>
		protected internal virtual void OnEnd() { /* Put after call to OnExecute() */ }

		/// <summary> Called once at final shutdown. Use to destruct. </summary>
		protected internal virtual void OnShutdown() { /* Call on agent? */ }
		#endregion

		//Handle disconnections automatically when node deleted in BHEditor
		protected virtual void OnDestroy()
		{
#if UNITY_EDITOR
			Debug.LogFormat("Destroying [{0}:{1}]", this.GetType().Name, this.ID);

			//Remove relationships from parent
			if (parent)
			{
				Undo.RecordObject(this.eData.owner, "Remove Child Node");
				parent.RemoveChild(this);
			}
#endif
			//UPDATE!! This can't be here
			//Remove from editor node references list
			// this.eData.owner.eNodes.Remove(this);
		}

// #if UNITY_EDITOR
		internal virtual Rect DrawNode(int id)	//Public?
		{
			//Update node's ID
			this.ID = id;

			return GUI.Window(id, eData.rect, DrawNodeContentCallback, eData.name, BhaVEditor.skin.window);
		}

		/// <summary> Draw the GUI items for this node </summary>
		public virtual void DrawContent() 
		{
			eData.name += ":" + ID;
		}

		/// <summary> Draw the connections of this node where applicable </summary>
		public abstract void DrawConnections();

		void DrawNodeContentCallback(int id)
		{
			DrawContent();

			//Handle node movement
			if (eData.isDraggable)
			{
				Undo.RecordObject(this, "Move Node");
				GUI.DragWindow();

				//Also move all children
			}
		}

		/// <summary> 
		/// Fail this node and all its children.
		/// This should not have any affect on the functionality of the tree
		/// and is ONLY for BhaVEditor's realtime visualisation of the nodes during play mode.
		/// </summary>
		public abstract void FailNodeAndChildren();
// #endif
	}
}