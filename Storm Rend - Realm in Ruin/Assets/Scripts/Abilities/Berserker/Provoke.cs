using UnityEngine;

/// <summary>
/// Berserker's Provoke ability
/// Lv1: Attract enemy aggro, deal 1 damage back to enemies on hit
/// Lv2: Attract enemy aggro, deal 2 damage back to enemies on hit
/// Lv3: Attract enemy aggro, deal 2 damage back to enemies on hit, gain rune of protection
/// </summary>
public class Provoke : Ability
{
    public void CastImmediately(AbilityLevel abilityLevel)
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
                    Retaliate(2);
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