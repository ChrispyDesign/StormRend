using BhaVE.Core;
using BhaVE.Delegates;
using BhaVE.Nodes;
using UnityEngine;

namespace StormRend.Bhaviours
{
    /// <summary>
    /// Finished the unit's turn
    /// </summary>
	// [CreateAssetMenu(menuName = "StormRend/Delegates/Actions/FinishUnitTurn", fileName = "FinishUnitTurn")]
    public class FinishUnitTurnAction : BhaveAction
    {
        public override NodeState Execute(BhaveAgent agent)
        {
			//Do nothing because the AI controller should take care of everything
			//Maybe just use the end behaviour tree instead
			return NodeState.Success;
        }
    }
}
