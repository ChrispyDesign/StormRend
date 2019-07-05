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
    [SerializeField] private UIAbilityManager m_abilityManager;

    #region getters

    public GloryManager GetGloryManager() { return m_gloryManager; }
    public BlizzardManager GetBlizzardManager() { return m_blizzardManager; }
    public UIAvatarSelector GetAvatarSelector() { return m_avatarSelector; }
    public UIAbilityManager GetAbilityManager() { return m_abilityManager; }

    #endregion
    
    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        Debug.Assert(m_blizzardManager, "Blizzard Manager not assigned to UI Manager!");
        Debug.Assert(m_gloryManager, "Glory Manager not assigned to UI Manager!");
        Debug.Assert(m_avatarSelector, "Avatar Manager not assigned to UI Manager!");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static UIManager GetInstance()
    {
        if (!m_instance)
            m_instance = FindObjectOfType<UIManager>();

        Debug.Assert(m_instance, "UI Manager not found!");

        return m_instance;
    }
}
