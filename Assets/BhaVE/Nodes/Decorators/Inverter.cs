using UnityEngine;
using BhaVE.Core;
using BhaVE.Editor;

namespace BhaVE.Nodes.Decorators
{
	// [CreateAssetMenu(menuName = "BhaVEInternal/Decorators/Inverter")]
	public sealed class Inverter : Decorator
	{
		protected internal override NodeState OnExecute(BhaveAgent agent)
		{
			switch (child.OnExecute(agent))
			{
#if UNITY_EDITOR   //For BHEditor live view and debugging
				//Bhave System Node States. Return Immediately
                case NodeState.None:
                    state = NodeState.None;
                    return state;
                case NodeState.Aborted:
                    state = NodeState.Aborted;
                    return state;
                case NodeState.Suspended:
                    state = NodeState.Suspended;
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
			eData.name = "Inverter";
			GUILayout.Label("-1", BhaVEditor.skin.label);
			base.DrawContent();
		}
#endif
	}
}