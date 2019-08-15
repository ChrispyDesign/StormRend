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
        [SerializeField] AIController aiController;

        public override NodeState Execute(BhaveAgent agent)
        {
            aiController.EndAITurn();
			return NodeState.Success;
        }
    }
}
