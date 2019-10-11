using pokoro.BhaVE.Core;
using pokoro.BhaVE.Core.Delegates;
using pokoro.BhaVE.Core.Enums;
using StormRend.Variables;
using UnityEngine;

namespace StormRend.Bhaviours
{
	/// <summary>
	/// 
	/// </summary>
	// [CreateAssetMenu(menuName = "StormRend/Delegates/Conditions/UnitsHaveRune", fileName = "UnitsHaveRune")]
	public class UnitsHaveRuneCondition : BhaveCondition
    {
		[SerializeField] UnitListVar targets;


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
