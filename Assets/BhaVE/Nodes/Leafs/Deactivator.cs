using BhaVE.Core;
#if UNITY_EDITOR
using BhaVE.Editor;
using UnityEngine;
#endif

namespace BhaVE.Nodes.Leafs
{
    /// <summary>
    /// Immediately terminates the current tree operation
    /// </summary>
    public class Deactivator : Leaf
    {
        protected internal override NodeState OnExecute(BhaveAgent agent)
        {
            agent.SetActive(false);
            state = NodeState.Aborted;
            return state;
        }

#if UNITY_EDITOR
        public override void DrawContent()
        {
            GUILayout.Label("X", BhaVEditor.skin.label);
        }
#endif
    }
}
