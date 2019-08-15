using BhaVE.Core;
using BhaVE.Delegates;
using BhaVE.Nodes;
using StormRend.AI;
using UnityEngine;

namespace StormRend.Bhaviours
{
    /// <summary>
    /// Finished the unit's turn
    /// </summary>
	[CreateAssetMenu(menuName = "StormRend/Delegates/Actions/FinishUnitTurn", fileName = "FinishUnitTurn")]
    public class FinishUnitTurnAction : BhaveAction
    {
        AIController aiController;

        public override void Initiate(BhaveAgent agent)
        {
            aiController = FindObjectOfType<AIController>();
        }

        public override NodeState Execute(BhaveAgent agent)
        {
            Debug.Log("FinishUnitTurnAction");
            
            // aiController.EndAITurn();
			return NodeState.Success;
        }
    }
}
