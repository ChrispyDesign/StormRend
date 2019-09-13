using System;
using UnityEngine;
using BhaVE.Core;
using BhaVE.Editor;

namespace BhaVE.Nodes.Composites
{
	public class Sequence : Composite
	{
		protected internal override NodeState OnExecute(BhaveAgent agent)
		{
			state = NodeState.Success;
			bool anyChildPending = false;

			foreach (var c in children)
			{
#if UNITY_EDITOR
				//If a Falure found then set the remaining children to Failure also
				if (state == NodeState.Failure)
				{
					c.SetNodeAndChildren(state);
					continue;   //Don't run anymore child Execute();
				}
#endif
				NodeState result = c.OnExecute(agent);
				switch (result)
				{
#if UNITY_EDITOR
					//Live status updates
					case NodeState.Aborted:
					case NodeState.Suspended:
					case NodeState.None:
						// SetNodeAndChildren(result);	//Cant use this, you wont see what happened
						state = result;
						return state;
#endif
					//Break out and also fail the remaining children
					case NodeState.Failure:
						state = NodeState.Failure;
						continue;
					//Continue with next child if success found
					case NodeState.Success:
						continue;
					//Set child running flag and continue
					case NodeState.Pending:
						anyChildPending = true;
						continue;

					default:
						throw new NotImplementedException("NodeState not implemented yet");
				}
			}

			//Return pending if any child was pending and now failures encountered
			if (anyChildPending && state != NodeState.Failure)
				state = NodeState.Pending;

			return state;
		}

#if UNITY_EDITOR
		public override void DrawContent()
		{
			GUILayout.Label("AND", BhaVEditor.skin.label);
		}
#endif
	}
}
