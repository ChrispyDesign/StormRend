using BhaVE.Core;
using BhaVE.Delegates;
using BhaVE.Nodes;
using BhaVE.Variables;
using UnityEngine;

namespace StormRend.Bhaviours
{
	// [CreateAssetMenu(menuName = "StormRend/Delegates/Conditions/UnitsHaveRune", fileName = "UnitsHaveRune")]
    public class UnitsHaveRuneCondition : BhaveCondition
    {
		[SerializeField] BhaveUnitList targets;
		Unit unit;

		public override void Initiate(BhaveAgent agent)
		{
			unit = agent.GetComponent<Unit>();
		}

		public override NodeState Execute(BhaveAgent agent)
        {
			Debug.Log("UnitsHaveRuneCondition");
			
            throw new System.NotImplementedException();
        }
    }
}
