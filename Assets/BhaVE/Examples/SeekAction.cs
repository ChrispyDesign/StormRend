using UnityEngine;
using pokoro.BhaVE.Core.Delegates;
using pokoro.BhaVE.Core;
using pokoro.BhaVE.Core.Variables;
using pokoro.BhaVE.Core.Enums;

namespace BhaVE.Delegates.Examples
{
	//Referenced from Opsive's Behaviour Designer's tutorials
	[System.Serializable]
	[CreateAssetMenu(menuName = "BhaVE/Delegates/Actions/Seek")]
	public class SeekAction : BhaveAction
	{
		public BhaveTransform target;
		public float speed = 5;

		public override NodeState Execute(BhaveAgent agent)
		{
			//if arrived at destination
			if (Vector3.SqrMagnitude(agent.transform.position - target.value.position) < 0.1f)
			{
				return NodeState.Success;
			}

			//Seek & look at target
			agent.transform.position = Vector3.MoveTowards(agent.transform.position, target.value.position, speed * Time.deltaTime);
			agent.transform.LookAt(target);

			return NodeState.Pending;
		}

		public override void Paused(bool paused)
		{
			Debug.Log("Agent paused! : "+ paused);
		}
	}
}