using UnityEngine;
using BhaVE.Core;
#if UNITY_EDITOR
using BhaVE.Editor;
#endif

namespace BhaVE.Nodes.Decorators
{
    /// <summary>
    /// Always return success
    /// </summary>
    public sealed class Succeeder : Decorator
    {
        [SerializeField] bool returnPending = false;
        protected internal override NodeState OnExecute(BhaveAgent agent)
        {
            NodeState result = child.OnExecute(agent);
            switch (result)
            {
#if UNITY_EDITOR
                //Live status updating
                case NodeState.Aborted:
                case NodeState.Suspended:
				case NodeState.None:
					state = result;
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
            GUILayout.Label("+", BhaVEditor.skin.label);
			returnPending = GUILayout.Toggle(returnPending, new GUIContent("Pending?", "Return pending if child node also pending?"), BhaVEditor.skin.toggle);
        }
#endif
    }
}