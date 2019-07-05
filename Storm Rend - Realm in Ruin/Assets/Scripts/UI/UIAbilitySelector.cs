using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
public class UIAbilitySelector : MonoBehaviour
{
    // panel for activation/deactivation
    [Header("Ability Button Panel")]
    [SerializeField] private GameObject m_buttonPanel;

    // ability buttons, passive, ability 1/2
    [Header("Ability Buttons")]
    [SerializeField] private Button m_passiveAbility;
    [SerializeField] private Transform m_ability1;
    [SerializeField] private Transform m_ability2;

    // helper variables
    private Button[] m_abilityButtons1;
    private Button[] m_abilityButtons2;

    /// <summary>
    /// cache button components
    /// </summary>
    void Start()
    {
        m_abilityButtons1 = m_ability1.GetComponentsInChildren<Button>();
        m_abilityButtons2 = m_ability2.GetComponentsInChildren<Button>();
    }

    /// <summary>
    /// when a player is selected, display the relevant ability elements
    /// </summary>
    /// <param name="playerUnit"></param>
    public void SelectPlayerUnit(PlayerUnit playerUnit)
    {
        if (playerUnit == null)
        {
            m_buttonPanel.SetActive(false);
            return;
        }

        m_buttonPanel.SetActive(true);
        
        Ability[] abilities = playerUnit.GetAbilities();

        if (abilities.Length == 2)
        {
            DisplayAbility(m_abilityButtons1, abilities[0]);
            DisplayAbility(m_abilityButtons2, abilities[1]);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="buttons"></param>
    /// <param name="ability"></param>
    private void DisplayAbility(Button[] buttons, Ability ability)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            Button abilityButton = buttons[i];
            AbilityLevelInfo levelInfo = ability.GetLevel(i);

            abilityButton.image.sprite = levelInfo.m_abilityIcon;

            if (GloryManager.GetGloryCount() >= levelInfo.m_gloryRequirement)
                abilityButton.interactable = true;
            else
                abilityButton.interactable = false;
        }
    }
}
