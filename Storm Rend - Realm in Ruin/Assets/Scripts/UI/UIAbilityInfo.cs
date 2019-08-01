using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
public class UIAbilityInfo : MonoBehaviour
{
    #region Variables

    [SerializeField] private UIAbilitySelector m_selector;
    [SerializeField] private GameObject m_infoPanel;
    [SerializeField] private Ability m_ability;

    private Unit m_player;

    #endregion

    #region GettersAndSetters
    public Ability GetAbility() { return m_ability; }

    public void SetAbility(Ability _ability) { m_ability = _ability; }
    #endregion


    private void Start()
    {
        m_selector = UIManager.GetInstance().GetAbilitySelector();
        m_infoPanel = m_selector.GetInfoPanel();
    }

    /// <summary>
    /// 
    /// </summary>
    public void HoverAbility()
    {
        m_player = PlayerController.GetCurrentPlayer();
        if (m_player != null)
        {
            PlayerController.SetCurrentMode(PlayerMode.ATTACK);
            m_selector.SetInfoPanelData();

            if(m_player.GetAttackNodes() != null &&
                m_player.GetAttackNodes().Count > 0)
                m_player.UnShowAttackTiles();

            m_ability.GetSelectableTiles(ref m_player);
            m_player.OnDeselect();
            m_player.ShowAttackTiles();
        }
        m_infoPanel.SetActive(true);
    }

    /// <summary>
    /// 
    /// </summary>
    public void UnhoverAbility()
    {
        if (!(PlayerController.GetIsAbilityLocked()))
        {
            PlayerController.SetCurrentMode(PlayerMode.MOVE);
            if (m_player != null)
            {
                m_player.UnShowAttackTiles();
            }
            m_infoPanel.SetActive(false);
        }
    }

    public void OnClickAbility()
    {
        if(m_player != null)
        {
            PlayerController.SetCurrentMode(PlayerMode.ATTACK);
            m_player.SetLockedAbility(m_ability);
            PlayerController.SetIsAbilityLocked(true); ;
            m_player.ShowAttackTiles();
        }
    }
}
