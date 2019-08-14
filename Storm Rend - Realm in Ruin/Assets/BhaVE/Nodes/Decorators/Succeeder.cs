using UnityEngine;
using BhaVE.Core;
using BhaVE.Editor;

namespace BhaVE.Nodes.Decorators
{
    /// <summary>
    /// Always return success
    /// </summary>
    [CreateAssetMenu(menuName = "BhaVEInternal/Decorators/Succeeder")]
    public sealed class Succeeder : Decorator
    {
        [SerializeField] bool returnPending = false;
        protected internal override NodeState OnExecute(BhaveAgent agent)
        {
            switch (child.OnExecute(agent))
            {
#if UNITY_EDITOR    //For BHEditor live view and debugging
				case NodeState.None:
					state = NodeState.None;
					return state;
#endif
                case NodeState.Failure:
                case NodeState.Success:
                    state = NodeState.Success;
                    return state;
                case NodeState.Pending:
                    state = returnPending == false ? NodeState.Success : NodeState.Pending;
                    return state;

                default: throw new System.InvalidOperationException();
            }
        }

#if UNITY_EDITOR
        public override void DrawContent()
        {
            eData.name = "Succeeder";
            GUILayout.Label("+", BhaVEditor.skin.label);
            base.DrawContent();
			returnPending = GUILayout.Toggle(returnPending, new GUIContent("Pending?", "Return pending if child node also pending?"), BhaVEditor.skin.toggle);
        }
#endif
    }
}