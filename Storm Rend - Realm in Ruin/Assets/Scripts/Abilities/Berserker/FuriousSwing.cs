using UnityEngine;

/// <summary>
/// Berserker's Furious Swing ability
/// Lv1: Deal 2 damage to an enemy, gain 1 glory
/// Lv2: Deal 2 damage to an enemy, refresh actions (move and attack again)
/// Lv3: Deal 4 damage to an enemy, refresh actions
/// </summary>
public class FuriousSwing : Ability
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="abilityLevel"></param>
    public void CastImmediately(AbilityLevel abilityLevel)
    {
        switch (abilityLevel)
        {
            case AbilityLevel.LEVEL_1:
                DoDamage(2);
                GloryManager.GainGlory(1);
                break;

            case AbilityLevel.LEVEL_2:
                if (GloryManager.SpendGlory(m_effectLevels[1].m_gloryRequirement))
                {
                    DoDamage(2);
                    RefreshActions();
                }
                break;

            case AbilityLevel.LEVEL_3:
                if (GloryManager.SpendGlory(m_effectLevels[2].m_gloryRequirement))
                {
                    DoDamage(4);
                    RefreshActions();
                }
                break;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="damage"></param>
    private void DoDamage(int damage)
    {
        Debug.Log("Doing " + damage + " damage");
    }

    /// <summary>
    /// 
    /// </summary>
    private void RefreshActions()
    {
        Debug.Log("Refreshing actions");
    }
}