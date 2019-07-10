using UnityEngine;

/// <summary>
/// singleton UI manager which houses relevant UI scripts
/// </summary>
public class UIManager : MonoBehaviour
{
    // singleton (uh oh)
    private static UIManager m_instance;

    // an assortment of different UI scripts/managers
    [Header("UI Components")]
    [SerializeField] private GloryManager m_gloryManager = null;
    [SerializeField] private BlizzardManager m_blizzardManager = null;
    [SerializeField] private UIAvatarSelector m_avatarSelector = null;
    [SerializeField] private UIAbilitySelector m_abilitySelector = null;
    [SerializeField] private UIAbilityInfo m_abilityInfo = null;

    #region getters

    public GloryManager GetGloryManager() { return m_gloryManager; }
    public BlizzardManager GetBlizzardManager() { return m_blizzardManager; }
    public UIAvatarSelector GetAvatarSelector() { return m_avatarSelector; }
    public UIAbilitySelector GetAbilitySelector() { return m_abilitySelector; }
    public UIAbilityInfo GetAbilityInfo() { return m_abilityInfo; }

    #endregion
    
    /// <summary>
    /// error handling
    /// </summary>
    void Start()
    {
        // ensure all of the relevant UI scripts are assigned
        Debug.Assert(m_blizzardManager, "Blizzard Manager not assigned to UI Manager!");
        Debug.Assert(m_gloryManager, "Glory Manager not assigned to UI Manager!");
        Debug.Assert(m_avatarSelector, "Avatar Manager not assigned to UI Manager!");
        Debug.Assert(m_abilitySelector, "Ability Selector not assigned to UI Manager!");
        Debug.Assert(m_abilityInfo, "Ability Info not assigned to UI Manager!");
    }

    /// <summary>
    /// single getter
    /// </summary>
    /// <returns>instance of this singleton class</returns>
    public static UIManager GetInstance()
    {
        // if no instance is assigned...
        if (!m_instance)
            m_instance = FindObjectOfType<UIManager>(); // find the instance

        // error handling
        Debug.Assert(m_instance, "UI Manager not found!");

        // done
        return m_instance;
    }
}