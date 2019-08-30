using System.Collections;
using System.Collections.Generic;
using StormRend;
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
        m_player = GameManager.singleton.GetPlayerController().GetCurrentPlayer();
        if (m_player != null)
        {
            GameManager.singleton.GetPlayerController().SetCurrentMode(PlayerMode.ATTACK);
            m_selector.SetInfoPanelData();

            if(m_player.GetAttackTiles() != null &&
                m_player.GetAttackTiles().Count > 0)
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
		bool isLockedAbility = GameManager.singleton.GetPlayerController().GetIsAbilityLocked();

		if(!isLockedAbility)
			GameManager.singleton.GetPlayerController().SetCurrentMode(PlayerMode.MOVE);

		Ability previousAbility = m_player.GetSelectedAbility();
		if (m_player != null && previousAbility != null)
		{
			if (previousAbility.GetTilesToSelect() == previousAbility.GetTiles().Count)
				m_player.UnShowAttackTiles();
		}
		m_infoPanel.SetActive(false);

		if (isLockedAbility)
		{
			Ability ability = GameManager.singleton.GetPlayerController().GetCurrentPlayer().GetSelectedAbility();
			Unit player = GameManager.singleton.GetPlayerController().GetCurrentPlayer() as Unit;
			if (!player)
			{
				ability.GetSelectableTiles(ref player);
			}
			player.ShowAttackTiles();
		}
	}

    public void OnClickAbility()
    {
		PlayerController controller = GameManager.singleton.GetPlayerController();

		Button button = this.gameObject.GetComponent<Button>();
        if(m_player != null && !m_player.GetHasAttacked() && button.interactable)
        {
			controller.SetCurrentMode(PlayerMode.ATTACK);
			m_ability.GetTiles().Clear();
            m_player.SetSelectedAbility(m_ability);
			controller.SetIsAbilityLocked(true); ;
            m_player.ShowAttackTiles();
		}
		m_infoPanel.SetActive(false);
	}
}
