using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StormRend.Anim.Behaviours
{
	public class DamageNumberReset : StateMachineBehaviour
	{
		// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
		public override void OnStateExit(UnityEngine.Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			animator.SetInteger("Damage", 0);
		}
	}
}
