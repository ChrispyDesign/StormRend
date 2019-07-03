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
    [SerializeField] private UIAvatarSelector m_avatarSelector;

    [Header("Ability Panel")]
    [SerializeField] private Text m_ability;
    [SerializeField] private Text m_descriptionLevel1;
    [SerializeField] private Text m_descriptionLevel2;
    [SerializeField] private Text m_descriptionLevel3;

    #region getters

    public static UIManager GetInstance() { return m_instance; }
    public GloryManager GetGloryManager() { return m_gloryManager; }
    public BlizzardManager GetBlizzardManager() { return m_blizzardManager; }
    public UIAvatarSelector GetAvatarSelector() { return m_avatarSelector; }

    #endregion
    
    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        m_instance = this;

        Debug.Assert(m_blizzardManager, "Blizzard Manager not assigned to UI Manager!");
        Debug.Assert(m_gloryManager, "Glory Manager not assigned to UI Manager!");
        Debug.Assert(m_avatarSelector, "Avatar Manager not assigned to UI Manager!");
    }

    public void SelectAbility()
    {

    }
}
