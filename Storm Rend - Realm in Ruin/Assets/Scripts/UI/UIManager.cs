using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager m_instance;

    [Header("Other UI Managers")]
    [SerializeField] private GloryManager m_gloryManager;
    [SerializeField] private BlizzardManager m_blizzardManager;

    [Header("Unit Avatars")]
    [SerializeField] private RectTransform m_berserkerAvatar;
    [SerializeField] private RectTransform m_valkyrieAvatar;
    [SerializeField] private RectTransform m_sageAvatar;
    [SerializeField] private float m_focusedScalar = 1.5f;

    [Header("Ability Panel")]
    [SerializeField] private Text m_ability;
    [SerializeField] private Text m_level1;
    [SerializeField] private Text m_level2;
    [SerializeField] private Text m_level3;

    private Vector2 m_defaultAvatarSize;
    private Vector2 m_focussedAvatarSize;

    #region getters

    public static UIManager GetInstance() { return m_instance; }
    public GloryManager GetGloryManager() { return m_gloryManager; }
    public BlizzardManager GetBlizzardManager() { return m_blizzardManager; }

    #endregion
    
    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        m_instance = this;

        Debug.Assert(m_blizzardManager != null, "Blizzard Manager not assigned to UI Manager!");
        Debug.Assert(m_gloryManager != null, "Glory Manager not assigned to UI Manager!");

        m_defaultAvatarSize = m_berserkerAvatar.rect.size;
        m_focussedAvatarSize = m_defaultAvatarSize * m_focusedScalar;
    }

    public void SelectAbility()
    {

    }

    public void SelectPlayerUnit(PlayerUnit playerUnit)
    {
        m_berserkerAvatar.sizeDelta = m_defaultAvatarSize;
        m_valkyrieAvatar.sizeDelta = m_defaultAvatarSize;
        m_sageAvatar.sizeDelta = m_defaultAvatarSize;

        switch (playerUnit.GetUnitType())
        {
            case UnitType.Berserker:
                m_berserkerAvatar.sizeDelta = m_focussedAvatarSize;
                break;

            case UnitType.Valkyrie:
                m_valkyrieAvatar.sizeDelta = m_focussedAvatarSize;
                break;

            case UnitType.Sage:
                m_sageAvatar.sizeDelta = m_focussedAvatarSize;
                break;
        }
    }
}
