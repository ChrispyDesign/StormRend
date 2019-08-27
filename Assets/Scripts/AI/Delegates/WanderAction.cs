using System.Collections;
using BhaVE.Core;
using BhaVE.Nodes;
using UnityEngine;

namespace BhaVE.Delegates.Examples
{
	[CreateAssetMenu(menuName = "BhaVE/Delegates/Actions/Wander")]
	public class WanderAction : BhaveAction
	{
		public float speed = 3f;
		public float directionChangeInterval = 1;
		public float maxHeadingChange = 30;

		float heading;
		Vector3 targetRotation;
		private float lastChangeTime = 0;

		public override void Initiate(BhaveAgent agent)
		{
			heading = Random.Range(0, 360);
		}

		public override NodeState Execute(BhaveAgent agent)
		{
			//If time between last change time is greater than 
			if (Time.time - lastChangeTime >= directionChangeInterval) 
				SetNewHeading();

			agent.transform.eulerAngles = Vector3.Slerp(agent.transform.eulerAngles, targetRotation, Time.deltaTime * directionChangeInterval);
			var forward = agent.transform.TransformDirection(Vector3.forward);
			agent.transform.position += forward * speed * Time.deltaTime;

			return NodeState.Success;
		}

		void SetNewHeading()
		{
			var floor = Mathf.Clamp(heading - maxHeadingChange, 0, 360);
			var ceil = Mathf.Clamp(heading + maxHeadingChange, 0, 360);
			heading = Random.Range(floor, ceil);
		   
			targetRotation = new Vector3(0, heading, 0);
		}
	}
}