using UnityEngine;
using BhaVE.Core;
#if UNITY_EDITOR
using BhaVE.Editor;
#endif

namespace BhaVE.Nodes.Decorators
{
	public sealed class Inverter : Decorator
	{
		protected internal override NodeState OnExecute(BhaveAgent agent)
		{
			NodeState result = child.OnExecute(agent);
			switch (result)
			{
#if UNITY_EDITOR
				//Live in-editor node status updates
				case NodeState.Aborted:
				case NodeState.Suspended:
                case NodeState.None:
                    state = result;
                    return state;
#endif
				case NodeState.Failure:
					state = NodeState.Success;
					return state;
				case NodeState.Success:
					state = NodeState.Failure;
					return state;
				case NodeState.Pending:
					state = NodeState.Pending;
					return state;

				default:
					throw new System.InvalidOperationException();
			}
		}

#if UNITY_EDITOR
		public override void DrawContent()
		{
			GUILayout.Label("-1", BhaVEditor.skin.label);
		}
#endif
	}
}