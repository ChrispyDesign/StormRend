using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAbilityManager : MonoBehaviour
{
    [Header("Ability Panels")]
    [SerializeField] private GameObject m_abilityPanel;
    [SerializeField] private GameObject m_abilityInfoPanel;

    [Header("Ability Buttons")]
    [SerializeField] private Button m_passiveAbility;
    [SerializeField] private Transform m_ability1;
    [SerializeField] private Transform m_ability2;

    private Button[] m_abilityButtons1;
    private Button[] m_abilityButtons2;

    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        m_abilityButtons1 = m_ability1.GetComponentsInChildren<Button>();
        m_abilityButtons2 = m_ability2.GetComponentsInChildren<Button>();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="playerUnit"></param>
    public void SelectPlayerUnit(PlayerUnit playerUnit)
    {
        if (playerUnit == null)
        {
            m_abilityPanel.SetActive(false);
            return;
        }

        m_abilityPanel.SetActive(true);

    }

    public void HoverAbility()
    {
        m_abilityInfoPanel.SetActive(true);
    }

    public void UnhoverAbility()
    {
        m_abilityInfoPanel.SetActive(false);
    }
}
