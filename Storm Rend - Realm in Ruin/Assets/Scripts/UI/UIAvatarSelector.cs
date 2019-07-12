using UnityEngine;

/// <summary>
/// avatar selection script responsible for resizing the avatars on player selection
/// </summary>
public class UIAvatarSelector : MonoBehaviour
{
    // avatar image/transform elements
    [Header("Unit Avatars")]
    [SerializeField] private RectTransform m_berserkerAvatar = null;
    [SerializeField] private RectTransform m_valkyrieAvatar = null;
    [SerializeField] private RectTransform m_sageAvatar = null;

    // the focused size of the avatar on selection
    [SerializeField] private float m_focusedScalar = 1.5f;
    
    // helper variables
    private Vector2 m_defaultAvatarSize;
    private Vector2 m_focusedAvatarSize;

    /// <summary>
    /// store the default avatar size and determine the 
    /// </summary>
    void Start()
    {
        m_defaultAvatarSize = m_berserkerAvatar.rect.size;
        m_focusedAvatarSize = m_defaultAvatarSize * m_focusedScalar;
    }
    
    /// <summary>
    /// use this to resize the avatar based on the currently selected player unit!
    /// </summary>
    /// <param name="playerUnit">the currently selected player unit</param>
    public void SelectPlayerUnit(PlayerUnit playerUnit)
    {
        // reset avatar sizes to default
        m_berserkerAvatar.sizeDelta = m_defaultAvatarSize;
        m_valkyrieAvatar.sizeDelta = m_defaultAvatarSize;
        m_sageAvatar.sizeDelta = m_defaultAvatarSize;

        // if no player unit is selected
        if (playerUnit == null)
            return; // don't do avatar focussing 

        switch (playerUnit.GetUnitType())
        {
            case PlayerClass.BERSERKER:
                m_berserkerAvatar.sizeDelta = m_focusedAvatarSize; // resize berserker avatar
                break;

            case PlayerClass.VALKYRIE:
                m_valkyrieAvatar.sizeDelta = m_focusedAvatarSize; // resize valkyrie avatar
                break;

            case PlayerClass.SAGE:
                m_sageAvatar.sizeDelta = m_focusedAvatarSize; // resize sage avatar
                break;
        }
    }
}