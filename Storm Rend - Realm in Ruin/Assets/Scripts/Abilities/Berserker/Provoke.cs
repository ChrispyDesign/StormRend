using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Provoke : Ability
{
    public override void CastImmediately(AbilityLevel abilityLevel)
    {
        switch (abilityLevel)
        {
            case AbilityLevel.LEVEL_1:
                Aggro();
                Retaliate(1);
                break;

            case AbilityLevel.LEVEL_2:
                if (GloryManager.SpendGlory(m_effectLevels[1].m_gloryRequirement))
                {
                    Aggro();
                    Retaliate(2);
                }
                break;

            case AbilityLevel.LEVEL_3:
                if (GloryManager.SpendGlory(m_effectLevels[2].m_gloryRequirement))
                {
                    Aggro();
                    GainRune();
                }
                break;
        }
    }

    private void Aggro()
    {
        Debug.Log("Gained Aggro");
    }

    private void Retaliate(int damage)
    {
        Debug.Log("Gained " + damage + " Retaliate");
    }

    private void GainRune()
    {
        Debug.Log("Gained Rune");
    }
}