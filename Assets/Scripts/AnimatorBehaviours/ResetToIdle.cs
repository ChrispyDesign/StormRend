using UnityEngine;

namespace StormRend.AnimatorBehaviours
{
    /// <summary>
    /// Go back to idle animation after this attack/cast animation has completed
    /// </summary>
    public class ResetToIdle : StateMachineBehaviour
    {
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetInteger("AttackAnim", 0);
        }
    }
}
