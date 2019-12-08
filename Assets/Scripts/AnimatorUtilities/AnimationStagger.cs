/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

using UnityEngine;

namespace StormRend.Anim.Behaviours
{ 
	public class AnimationStagger : StateMachineBehaviour
	{
		//Inspector
		[Tooltip("+/- range of seconds to offset animation by")]
		[SerializeField] float cycleOffsetRandomRange = 3f;

		[Tooltip("+/- multiplier to speed up or slow down animation by")]
		[SerializeField] float speedMultRandomRange = 0.3f;

		[SerializeField] string speedParam = "IdleSpeedMult";
		[SerializeField] string cycleParam = "IdleCycleOffset";

		//Members
		float speedMult;		//Multiplier
		float cycleOffset;		//Seconds

		public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
		{
		}

		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			speedMult = 1f + Random.Range(-speedMultRandomRange * 0.5f, +speedMultRandomRange * 0.5f);
			cycleOffset = Random.Range(-cycleOffsetRandomRange * 0.5f, +cycleOffsetRandomRange * 0.5f);

			animator.SetFloat(speedParam, speedMult);
			animator.SetFloat(cycleParam, cycleOffset);
		}
   	}
}