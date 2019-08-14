using System;
using UnityEngine;
using BhaVE.Core;
using BhaVE.Editor;

namespace BhaVE.Nodes.Composites
{
	// [CreateAssetMenu(menuName = "BhaVEInternal/Composites/Selector")]
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
					c.FailNodeAndChildren();
					continue;   //Don't run anymore child Executes();
				}
#endif
				switch (c.OnExecute(agent))
				{
#if UNITY_EDITOR   //For BHEditor live view and debugging
					case NodeState.None:
						state = NodeState.None;
						return state;
#endif
					//Continue on with next child if failure
					case NodeState.Failure:
						continue;

					//Return success instantly if found
					case NodeState.Success:
						state = NodeState.Success;
						break;

					//Return pending only after no successes found
					case NodeState.Pending:
						state = NodeState.Pending;
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
			eData.name = "Selector";
			GUILayout.Label("OR", BhaVEditor.skin.label);
			base.DrawContent();
		}
#endif
	}
}
