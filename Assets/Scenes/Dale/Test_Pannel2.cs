﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Pannel2 : StateMachineBehaviour
{


    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.SetActive(false);
        animator.SetBool("Done", false);
        animator.SetInteger("Amount", 0);

    }


}
