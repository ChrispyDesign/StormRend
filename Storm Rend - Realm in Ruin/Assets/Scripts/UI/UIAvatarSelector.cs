using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAvatarSelector : MonoBehaviour
{
    [Header("Unit Avatars")]
    [SerializeField] private RectTransform m_berserkerAvatar;
    [SerializeField] private RectTransform m_valkyrieAvatar;
    [SerializeField] private RectTransform m_sageAvatar;
    [SerializeField] private float m_focusedScalar = 1.5f;

    private Vector2 m_defaultAvatarSize;
    private Vector2 m_focussedAvatarSize;

    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        m_defaultAvatarSize = m_berserkerAvatar.rect.size;
        m_focussedAvatarSize = m_defaultAvatarSize * m_focusedScalar;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="playerUnit"></param>
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
