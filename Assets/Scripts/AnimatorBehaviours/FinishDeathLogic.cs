using UnityEngine;

namespace StormRend.AnimatorBehaviours
{
    /// <summary>
    /// Deactivates the unit after the fade out death animation (this state) finishes playing
    /// </summary>
    public class FinishDeathLogic : StateMachineBehaviour
    {
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.transform.root.gameObject.SetActive(false);
        }
    }
}
