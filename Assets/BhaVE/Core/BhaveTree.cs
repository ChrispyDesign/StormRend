using System;
using System.Collections.Generic;
using BhaVE.Nodes;
using UnityEngine;

namespace BhaVE.Core
{
	//Brainstorm
	//What is this class? What does it do?
	// - Scriptable object
	// - Inbuilt into each agent
	// - Can also be created as an external module file and injected into an agent
	// - root node holds the tree; all the node in tree format
	// - eNodes holds all references to every node for easier drawing in the BHEditor
	//API
	// - Method to run and evaluate this tree
	// - (Protected?) methods to run each node's OnActivate()
	// - Methods to run Node.OnActivate(), OnBegin(), OnEnd(), OnPause(), etc. These are only invoked by it's agent and not this scriptable object as Bhave tree cannot operate without an agent.
	// - Editor accessible method to clear the entire tree

	[Serializable, CreateAssetMenu(menuName = "BhaVE/Tree", fileName = "BhaveTree")]
	public sealed class BhaveTree : ScriptableObject
	{
		[SerializeField] internal Node root = null;     //Allow BhaVEditor & inspector access

		#region Core Methods
		//------------ Inits -------------
		/// <summary> To be called once at Awake() time. Use to construct </summary>
		internal void RunAwaken(BhaveAgent agent) => root.OnAwaken(agent);
		/// <summary> To be called once at Start() time. Use to construct </summary>
		internal void RunInitiate(BhaveAgent agent) => root.OnInitiate(agent);
		//----------- Main ---------------
		/// <summary> To be called once each tick cycle BEFORE this tree is evaluated </summary>
		internal void RunBegin() => root.OnBegin();
		/// <summary>
		/// Evaluates this tree and returns a node state value
		/// </summary>
		/// <param name="agent">The agent must be passed into this function</param>
		internal NodeState RunExecute(BhaveAgent agent) => root.OnExecute(agent);
		/// <summary>
		/// To be called according to the tick interval settings when agent is on pause
		/// </summary>
		/// <param name="paused"></param>
		internal void RunPause(bool paused) => root.OnPause(paused);
		/// <summary> To be called once each tick cycle AFTER this tree is evaluated </summary>
		internal void RunEnd() => root.OnEnd();
		//----------- Shutdowns -------------
		/// <summary> To be called at OnDestroy() time of this tree's owner </summary>
		internal void RunShutdown() => root.OnShutdown();
		#endregion

#if UNITY_EDITOR
		/// <summary>
		/// List of references for all the nodes in this tree
		/// Only used for drawing the nodes in editor between BeginWindow()/EndWindow() pair
		/// </summary>
		[SerializeField] internal List<Node> eNodes = new List<Node>();

		internal void Clear()
		{
			eNodes.Clear();
			root = null;
		}
#endif
	}
}