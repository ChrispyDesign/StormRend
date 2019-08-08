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
    [SerializeField] private GameObject m_buttonPanel = null;

    // ability buttons, passive, ability 1/2
    [Header("Ability Buttons")]
    [SerializeField] private RectTransform m_passivePanel;
    [SerializeField] private RectTransform m_firstAbilityPanel;
    [SerializeField] private RectTransform m_secondAbilityPanel;

    // panel for activation/deactivation
    [Header("Ability Info Panel")]
    [SerializeField] private GameObject m_infoPanel = null;

    // ability info text elements
    [Header("Ability Text Elements")]
    [SerializeField] private Text m_abilityTitle;
    [SerializeField] private Text m_abilityLevel1;
    [SerializeField] private Text m_abilityLevel2;
    [SerializeField] private Text m_abilityLevel3;

    [Header("Ability Individual Buttons")]
    [SerializeField] private Button m_passiveButton = null;
    [SerializeField] private Button[] m_firstAbilityButtons = null;
    [SerializeField] private Button[] m_secondAbilityButtons = null;

    private Ability m_passiveAbility;
    private Ability[] m_firstAbilities;
    private Ability[] m_secondAbilities;

    #region GettersAndSetters

    public GameObject GetInfoPanel() { return m_infoPanel; }

    #endregion


    /// <summary>
    /// cache button components
    /// </summary>
    void Start()
    {
        m_buttonPanel.SetActive(false);
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

        PlayerUnit player = GameManager.GetInstance().GetPlayerController().GetCurrentPlayer();

        if (player != null)
        {
            player.GetAbilities(ref m_passiveAbility,
                ref m_firstAbilities, ref m_secondAbilities);

            DisplayAbility(m_passiveButton, m_passiveAbility);

            for (int i = 0; i < m_firstAbilities.Length; i++)
                DisplayAbility(m_firstAbilityButtons[i], m_firstAbilities[i]);

            for (int i = 0; i < m_secondAbilities.Length; i++)
                DisplayAbility(m_secondAbilityButtons[i], m_secondAbilities[i]);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="buttons"></param>
    /// <param name="ability"></param>
    private void DisplayAbility(Button button, Ability ability)
    {
        button.GetComponent<UIAbilityInfo>().SetAbility(ability);
        button.image.sprite = ability.GetIcon();

        if (GloryManager.gloryCount >= ability.GetGloryRequirement())
            button.interactable = true;
        else
            button.interactable = false;

		PlayerUnit player = GameManager.GetInstance().GetPlayerController().GetCurrentPlayer();
		if (player.GetAlreadyAttacked())
			button.interactable = false;
    }

    public void SetInfoPanelData()
    {
        Vector2 rect = m_passivePanel.InverseTransformPoint(Input.mousePosition);
        if (m_passivePanel.rect.Contains(rect))
        {
            m_abilityTitle.text = m_passiveAbility.GetName();
            m_abilityLevel1.text = m_passiveAbility.GetDescription();
            m_abilityLevel2.text = "";
            m_abilityLevel3.text = "";
        }

        rect = m_firstAbilityPanel.InverseTransformPoint(Input.mousePosition);
        if (m_firstAbilityPanel.rect.Contains(rect))
        {
            m_abilityTitle.text = m_firstAbilities[0].GetName();
            m_abilityLevel1.text = m_firstAbilities[0].GetDescription();
            m_abilityLevel2.text = m_firstAbilities[1].GetDescription();
            m_abilityLevel3.text = m_firstAbilities[2].GetDescription();
        }

        rect = m_secondAbilityPanel.InverseTransformPoint(Input.mousePosition);
        if (m_secondAbilityPanel.rect.Contains(rect))
        {
            m_abilityTitle.text = m_secondAbilities[0].GetName();
            m_abilityLevel1.text = m_secondAbilities[0].GetDescription();
            m_abilityLevel2.text = m_secondAbilities[1].GetDescription();
            m_abilityLevel3.text = m_secondAbilities[2].GetDescription();
        }
    }
}
