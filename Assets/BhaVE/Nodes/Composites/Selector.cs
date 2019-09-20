using System;
using UnityEngine;
using BhaVE.Core;
#if UNITY_EDITOR
using BhaVE.Editor;
#endif

namespace BhaVE.Nodes.Composites
{
	public class Selector : Composite
	{
		protected internal override NodeState OnExecute(BhaveAgent agent)
		{
			state = NodeState.Failure;

			foreach (var c in children)
			{
#if UNITY_EDITOR
				//If a Success or Pending is found then set the remaining children to Failure
				if (state == NodeState.Success || state == NodeState.Pending)
				{
					c.SetNodeAndChildren(NodeState.Failure);
					continue;   //Don't run anymore child Executes();
				}
#endif
				NodeState result = c.OnExecute(agent);
				switch (result)
				{
// #if UNITY_EDITOR
	//These are required for deactivators and suspender nodes
					//Live status updates
					case NodeState.Aborted:
					case NodeState.Suspended:
					case NodeState.None:
						// SetNodeAndChildren(result);	//Cant use this, you wont see what happened
						state = result;
						return state;
// #endif
					case NodeState.Failure:		//Continue on with next child if failure
						continue;
					case NodeState.Success:		//Return success instantly if found
					case NodeState.Pending:		//Return pending only after no successes found
						state = result;
						break;

					default:
						throw new NotImplementedException("NodeState not implemented yet");
				}
			}
			return state;
		}

#if UNITY_EDITOR
		public override void DrawContent()
		{
			GUILayout.Label("OR", BhaVEditor.skin.label);
		}
#endif
	}
}
