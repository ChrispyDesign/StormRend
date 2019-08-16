using BhaVE.Core;
using BhaVE.Editor;
using UnityEngine;

namespace BhaVE.Nodes.Leafs
{
    /// <summary>
    /// Simple node that immediately terminates the current tree operation
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
            eData.name = "Deactivator";
            GUILayout.Label("X", BhaVEditor.skin.label);
            base.DrawContent();
        }
#endif
    }
}
