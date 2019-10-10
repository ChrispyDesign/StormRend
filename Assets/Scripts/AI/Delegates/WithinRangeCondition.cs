using UnityEngine;
using pokoro.BhaVE.Core.Delegates;
using pokoro.BhaVE.Core.Variables;
using pokoro.BhaVE.Core;
using pokoro.BhaVE.Core.Enums;

namespace BhaVE.Delegates.Examples
{
	[System.Serializable]
	[CreateAssetMenu(menuName = "BhaVE/Delegates/Conditions/WithinRange")]
	public class WithinRangeCondition : BhaveCondition
	{
		public BhaveTransform target;	//Shared transform that is accessible by other delegates
		public float range = 2f;
		public float FOV_angle = 100f;
		
		float distFromTarget;

		public override NodeState Execute(BhaveAgent agent)
		{
			// Debug.LogFormat("WithinRange.Execute(), distFromTarget: {0}", distFromTarget);

			//If target within range return success
			if (WithinRange(target, agent))
			{
				return NodeState.Success;
			}
			//else failure
			return NodeState.Failure;
		}

		public bool WithinRange(Transform targetTranform, BhaveAgent agent)
		{
			var dir = targetTranform.position - agent.transform.position;

			distFromTarget = Vector3.Distance(targetTranform.position, agent.transform.position);

			return Vector3.Angle(dir, agent.transform.forward) < FOV_angle && distFromTarget < range;
		}
	}
}
