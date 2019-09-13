using BhaVE.Core;
#if UNITY_EDITOR
using BhaVE.Editor;
using UnityEngine;
#endif

namespace BhaVE.Nodes.Leafs
{
    /// <summary>
    /// Immediately pauses the agent
    /// </summary>
    public class Suspender : Leaf
    {
        protected internal override NodeState OnExecute(BhaveAgent agent)
        {
            agent.SetPaused(true);
            state = NodeState.Suspended;
            return state;
        }

#if UNITY_EDITOR
        public override void DrawContent()
        {
            GUILayout.Label("||", BhaVEditor.skin.label);
        }
#endif
    }
}
