using BhaVE.Core;
using BhaVE.Delegates;
using BhaVE.Nodes;
using UnityEngine;

namespace StormRend.Bhaviours
{
    /// <summary>
    /// Stops the agent's behaviour tree
    /// </summary>
    [CreateAssetMenu(menuName = "StormRend/BhaVE/Actions/Stop Bhaviour", fileName = "StopBhaviour")]
    public class StopBhaviourAction : BhaveAction
    {
        public override NodeState Execute(BhaveAgent agent)
        {
            agent.SetActive(false);
            return NodeState.Success;
        }
    }
}
