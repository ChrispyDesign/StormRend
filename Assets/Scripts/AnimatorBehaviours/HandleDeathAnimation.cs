using StormRend.Units;
using UnityEngine;

namespace StormRend.AnimatorBehaviours
{
    /// <summary>
    /// Checks if the unit has died (after this HitReact animation) and if so proceed to trigger the death animation
    /// </summary>
    public class HandleDeathAnimation : StateMachineBehaviour
    {
		[SerializeField] string deathParam = "isDead";
        AnimateUnit au;   //The unit attached to this animator
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
			if (!au) au = animator.GetComponentInParent<AnimateUnit>();		//get once?
            //If unit has died then go int death animation
			animator.SetBool(deathParam, au.isDead);
        }
    }
}
