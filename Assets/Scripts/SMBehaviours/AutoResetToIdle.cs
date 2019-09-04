using UnityEngine;

namespace StormRend.SMB
{
    public class AutoResetToIdle : StateMachineBehaviour
    {
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetInteger("AttackAnim", 0);
        }
    }
}
