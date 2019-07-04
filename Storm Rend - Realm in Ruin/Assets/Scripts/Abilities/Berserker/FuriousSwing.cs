using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuriousSwing : Ability
{
    public override void CastImmediately(AbilityLevel abilityLevel)
    {
        switch (abilityLevel)
        {
            case AbilityLevel.LEVEL_1:
                DoDamage(2);
                GloryManager.GainGlory(1);
                break;

            case AbilityLevel.LEVEL_2:
                if (GloryManager.SpendGlory(m_effectLevel2.m_gloryRequirement))
                {
                    DoDamage(2);
                    RefreshActions();
                }
                break;

            case AbilityLevel.LEVEL_3:
                if (GloryManager.SpendGlory(m_effectLevel3.m_gloryRequirement))
                {
                    DoDamage(4);
                    RefreshActions();
                }
                break;
        }
    }

    public override void CastDelayed(ExecutionOrder executionOrder, int turnDelay, int level = 1)
    {

    }

    private void DoDamage(int damage)
    {
        Debug.Log("Doing " + damage + " damage");
    }

    private void RefreshActions()
    {
        Debug.Log("Refreshing actions");
    }
}
