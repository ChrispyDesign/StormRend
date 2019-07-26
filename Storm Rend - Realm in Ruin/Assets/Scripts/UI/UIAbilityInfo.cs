using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
public class UIAbilityInfo : MonoBehaviour
{
    // panel for activation/deactivation
    [Header("Ability Info Panel")]
    [SerializeField] private GameObject m_infoPanel = null;

    // ability info text elements
    [Header("Ability Text Elements")]
    [SerializeField] private Text m_abilityTitle;
    [SerializeField] private Ability m_abilityLevel1;
    [SerializeField] private Ability m_abilityLevel2;
    [SerializeField] private Ability m_abilityLevel3;

    #region GettersAndSetters

    #endregion

    /// <summary>
    /// 
    /// </summary>
    public void HoverAbility()
    {
        m_infoPanel.SetActive(true);
    }

    /// <summary>
    /// 
    /// </summary>
    public void UnhoverAbility()
    {
        m_infoPanel.SetActive(false);
    }
}
