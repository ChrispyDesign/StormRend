using System;
using UnityEngine;
using BhaVE.Core;
using BhaVE.Editor;

namespace BhaVE.Nodes.Composites
{
	[CreateAssetMenu(menuName = "BhaVEInternal/Composites/Sequence")]
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
					c.FailNodeAndChildren();
					continue;   //Don't run anymore child Execute();
				}
#endif
				switch (c.OnExecute(agent))
				{
#if UNITY_EDITOR   //For BHEditor live view and debugging
					case NodeState.None:
						state = NodeState.None;
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
			eData.name = "Sequence";
			GUILayout.Label("AND", BhaVEditor.skin.label);
			base.DrawContent();
		}
#endif
	}
}
