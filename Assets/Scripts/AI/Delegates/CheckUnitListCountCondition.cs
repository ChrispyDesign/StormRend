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
	[CreateAssetMenu(menuName = "BhaVE/Delegates/Conditions/CheckUnitListCount", fileName = "CheckUnitListCount")]
    public class CheckUnitListCountCondition : BhaveCondition
    {
		public enum ComparisonType
		{ 
			LessThan,
			LessOrEqual,
			Equal,
			GreaterThan,
			GreaterOrEqual
		}
		[SerializeField] ComparisonType comparisonType;
		[SerializeField] BhaveUnitList targets;
		[SerializeField] int countToCompare = 5;

        public override NodeState Execute(BhaveAgent agent)
        {
			switch (comparisonType)
			{
				case ComparisonType.LessThan:
					if (targets.value.Count < countToCompare) 
					return NodeState.Success;
					break;
				case ComparisonType.LessOrEqual:
					if (targets.value.Count <= countToCompare) 
					return NodeState.Success;
					break;
				case ComparisonType.Equal:
					if (targets.value.Count <= countToCompare) 
					return NodeState.Success;
					break;
				case ComparisonType.GreaterThan:
					if (targets.value.Count > countToCompare) 
					return NodeState.Success;
					break;
				case ComparisonType.GreaterOrEqual:
					if (targets.value.Count >= countToCompare) 
					return NodeState.Success;
					break;
			}
			return NodeState.Failure;
        }
    }
}
