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
