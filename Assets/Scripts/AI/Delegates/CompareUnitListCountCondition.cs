using BhaVE.Core;
using BhaVE.Delegates;
using BhaVE.Nodes;
using BhaVE.Variables;
using UnityEngine;

namespace StormRend.Bhaviours
{
    /// <summary>
	/// Checks the size of the target list
    /// </summary>
	[CreateAssetMenu(menuName = "StormRend/Delegates/Conditions/CompareUnitListCount", fileName = "CompareUnitListCount")]
    public class CompareUnitListCountCondition : BhaveCondition
    {
		public enum ComparisonMode
		{ 
			LessThan,
			LessOrEqual,
			Equal,
			GreaterThan,
			GreaterOrEqual
		}
		[SerializeField] ComparisonMode comparisonMethod;
		[SerializeField] BhaveUnitList targets;
		[SerializeField] int countToCompare = 5;

        public override NodeState Execute(BhaveAgent agent)
        {
			switch (comparisonMethod)
			{
				case ComparisonMode.LessThan:
					if (targets.value.Count < countToCompare) 
					return NodeState.Success;
					break;
				case ComparisonMode.LessOrEqual:
					if (targets.value.Count <= countToCompare) 
					return NodeState.Success;
					break;
				case ComparisonMode.Equal:
					if (targets.value.Count <= countToCompare) 
					return NodeState.Success;
					break;
				case ComparisonMode.GreaterThan:
					if (targets.value.Count > countToCompare) 
					return NodeState.Success;
					break;
				case ComparisonMode.GreaterOrEqual:
					if (targets.value.Count >= countToCompare) 
					return NodeState.Success;
					break;
			}
			return NodeState.Failure;
        }
    }
}
