using StormRend.Abilities;
using StormRend.UI;
using UnityEngine;
using UnityEngine.UI;

namespace StormRend.Defunct
{
	public class xUIAbilitySelector : MonoBehaviour
	{
		// panel for activation/deactivation
		[Header("Ability Button Panel")]
		[SerializeField] private GameObject m_buttonPanel = null;

		// ability buttons, passive, ability 1/2
		[Header("Ability Buttons")]
		[SerializeField] private RectTransform m_passivePanel = null;
		[SerializeField] private RectTransform m_firstAbilityPanel = null;
		[SerializeField] private RectTransform m_secondAbilityPanel = null;

		// panel for activation/deactivation
		[Header("Ability Info Panel")]
		[SerializeField] private GameObject m_infoPanel = null;

		// ability info text elements
		[Header("Ability Text Elements")]
		[SerializeField] private Text m_abilityTitle = null;
		[SerializeField] private Text m_abilityLevel1 = null;
		[SerializeField] private Text m_abilityLevel2 = null;
		[SerializeField] private Text m_abilityLevel3 = null;

		[Header("Ability Individual Buttons")]
		[SerializeField] private Button m_passiveButton = null;
		[SerializeField] private Button[] m_firstAbilityButtons = null;
		[SerializeField] private Button[] m_secondAbilityButtons = null;

		private xAbility m_passiveAbility = null;
		private xAbility[] m_firstAbilities = null;
		private xAbility[] m_secondAbilities = null;

		#region GettersAndSetters

		public GameObject GetInfoPanel() { return m_infoPanel; }
		public GameObject GetButtonPanel() { return m_buttonPanel; }

		#endregion


		/// <summary>
		/// cache button components
		/// </summary>
		void Start()
		{
			m_buttonPanel.SetActive(false);
		}

		/// <summary>
		/// when a player is selected, display the relevant ability elements
		/// </summary>
		/// <param name="playerUnit"></param>
		public void SelectPlayerUnit(xPlayerUnit playerUnit)
		{
			if (playerUnit == null)
			{
				m_buttonPanel.SetActive(false);
				return;
			}

			m_buttonPanel.SetActive(true);

			xPlayerUnit player = xGameManager.current.GetPlayerController().GetCurrentPlayer();

			if (player != null)
			{
				player.GetAbilities(ref m_passiveAbility,
					ref m_firstAbilities, ref m_secondAbilities);

				DisplayAbility(m_passiveButton, m_passiveAbility);

				for (int i = 0; i < m_firstAbilities.Length; i++)
					DisplayAbility(m_firstAbilityButtons[i], m_firstAbilities[i]);

				for (int i = 0; i < m_secondAbilities.Length; i++)
					DisplayAbility(m_secondAbilityButtons[i], m_secondAbilities[i]);
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="buttons"></param>
		/// <param name="ability"></param>
		private void DisplayAbility(Button button, xAbility ability)
		{
			button.GetComponent<xUIAbilityInfo>().SetAbility(ability);
			button.image.sprite = ability.GetIcon();

			if (xGloryManager.gloryCount >= ability.GetGloryRequirement())
				button.interactable = true;
			else
				button.interactable = false;

			xPlayerUnit player = xGameManager.current.GetPlayerController().GetCurrentPlayer();
			if (player.GetHasAttacked())
				button.interactable = false;
		}

		public void SetInfoPanelData()
		{
			Vector2 rect = m_passivePanel.InverseTransformPoint(Input.mousePosition);
			if (m_passivePanel.rect.Contains(rect))
			{
				m_abilityTitle.text = m_passiveAbility.name;
				m_abilityLevel1.text = m_passiveAbility.GetDescription();
				m_abilityLevel2.text = "";
				m_abilityLevel3.text = "";
			}

			rect = m_firstAbilityPanel.InverseTransformPoint(Input.mousePosition);
			if (m_firstAbilityPanel.rect.Contains(rect))
			{
				m_abilityTitle.text = m_firstAbilities[0].name;
				m_abilityLevel1.text = m_firstAbilities[0].GetDescription();
				m_abilityLevel2.text = m_firstAbilities[1].GetDescription();
				m_abilityLevel3.text = m_firstAbilities[2].GetDescription();
			}

			rect = m_secondAbilityPanel.InverseTransformPoint(Input.mousePosition);
			if (m_secondAbilityPanel.rect.Contains(rect))
			{
				m_abilityTitle.text = m_secondAbilities[0].name;
				m_abilityLevel1.text = m_secondAbilities[0].GetDescription();
				m_abilityLevel2.text = m_secondAbilities[1].GetDescription();
				m_abilityLevel3.text = m_secondAbilities[2].GetDescription();
			}
		}
	}
}