using UnityEngine;
using BhaVE.Core;
#if UNITY_EDITOR
using BhaVE.Editor.Nodes;
using BhaVE.Editor.Data;
using UnityEditor;
#endif

namespace BhaVE.Nodes
{
    [System.Serializable]
	public abstract class Node : ScriptableObject, IBHNode
#if UNITY_EDITOR
	, IBHENode
#endif
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

#if UNITY_EDITOR
		//Children
		// - This need to be implemented by Composite and Decorators
		// - All other nodes will have a null implementation
		public abstract bool hasChild { get; }
		public abstract void AddChild(Node n);
		public abstract void RemoveChild(Node n);
		public abstract bool ContainsChild(Node n);
#endif

		#region Core Methods
		// - These should be implemented by Action and Condition nodes
		// - Accessible by: Inherited nodes, Editor
		// - The virtual methods needs to run its child node(s) where necessary
		// -------------- Inits ----------------
		/// <summary> Called once at Awake() time. Use to construct </summary>
		protected internal virtual void OnAwaken(BhaveAgent agent) { }
		/// <summary> Called once at Start() time. Use to construct </summary>
		protected internal virtual void OnInitiate(BhaveAgent agent) { }
		// -------------- Mains ---------------
		/// <summary> Called once each tick cycle BEFORE node evaluation </summary>
		protected internal virtual void OnBegin() { }
		/// <summary>
		/// Evaluates the node returning a node state value.
		/// Call frequency according to system tick interval settings
		/// </summary>
		protected internal abstract NodeState OnExecute(BhaveAgent agent);
		/// <summary> Called once each tick cycle if agent is set to paused </summary>
		protected internal virtual void OnPause(bool paused) { }
		/// <summary> Called once each tick cycle AFTER node evaluation </summary>
		protected internal virtual void OnEnd() { }
		//--------------- Shutdowns -------------
		/// <summary> Called once at OnDestroy() time. Use to destruct and cleanup </summary>
		protected internal virtual void OnShutdown() { }
		#endregion

#if UNITY_EDITOR
		//Handle disconnections automatically when node deleted in BHEditor
		protected virtual void OnDestroy()
		{
			Debug.LogFormat("Destroying [{0}:{1}]", this.GetType().Name, this.ID);

			//Remove relationships from parent
			if (parent)
			{
				Undo.RecordObject(this.eData.owner, "Remove Child Node");
				parent.RemoveChild(this);
			}

			//UPDATE!! This can't be here
			//Remove from editor node references list
			// this.eData.owner.eNodes.Remove(this);
		}

		public virtual Rect DrawNode(int id, Rect idealRect, GUIStyle style = null)	//Public?
		{
			//Q. How could this be improved?
			//A. BHE is in an editor folder and in it's own assembly

			if (style == null) style = GUIStyle.none;

			return GUI.Window(id, idealRect, DrawNodeContentCallback, this.name, style);
		}

		/// <summary> Draw the GUI items for this node </summary>
		public virtual void DrawContent() { }

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
		public abstract void SetNodeAndChildren(NodeState state);
#endif
	}
}