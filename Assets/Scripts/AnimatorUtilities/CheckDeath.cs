/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

using StormRend.Units;
using UnityEngine;

namespace StormRend.Anim.Behaviours
{
    /// <summary>
    /// Checks if the unit has died (after this HitReact animation) and if so proceed to trigger the death animation
    /// </summary>
    public class CheckDeath : StateMachineBehaviour
    {
		[Header("Sets death param to true if unit has died")]
		[Tooltip("Hence, triggering the death animation where appropriate")]
		[SerializeField] string deathParam = "isDead";
        AnimateUnit au;   //The unit attached to this animator

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
			if (!au) au = animator.GetComponentInParent<AnimateUnit>();
            //If unit has died then go int death animation
			animator.SetBool(deathParam, au.isDead);
        }
    }
}
