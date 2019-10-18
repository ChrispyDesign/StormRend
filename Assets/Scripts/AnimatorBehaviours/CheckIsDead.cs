using StormRend.Units;
using UnityEngine;

namespace StormRend.AnimatorBehaviours
{
    /// <summary>
    /// Checks if the unit has died (after this HitReact animation) and if so proceed to trigger the death animation
    /// </summary>
    public class CheckIsDead : StateMachineBehaviour
    {
        AnimateUnit unit;   //The unit attached to this animator
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            unit = animator.GetComponentInParent<AnimateUnit>();

            //Check if the unit is dead after being hit
            if (unit.isDead)
            {
                //If so then run death animation
                animator.SetTrigger("isDead");
            }
            else
            {
                animator.SetInteger("AttackAnim", 0);
            }
        }
    }
}
