using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

namespace StormRend.UI
{
	public class AvatarSelectButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		[SerializeField] string title;
		[SerializeField] string details;

		List<AbilitySelectButton> abilitySelectButtons;
		InfoPanel infoPanel;

		void Awake()
		{
			infoPanel = FindObjectOfType<InfoPanel>();
			Debug.Assert(infoPanel, "There are no Info Panel Script in the scene. " + typeof(FinishTurn));

			abilitySelectButtons = new List<AbilitySelectButton>();
			abilitySelectButtons.AddRange(FindObjectsOfType<AbilitySelectButton>());

			foreach (AbilitySelectButton t in abilitySelectButtons)
			{
				t.GetComponent<Image>().enabled = false;
			}
		}

		public void ShowAbilities()
		{
			foreach(AbilitySelectButton t in abilitySelectButtons)
			{
				t.GetComponent<Image>().enabled = true;
			}
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			infoPanel.ShowPanel(title, details);
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			infoPanel.UnShowPanel();
		}
	}
}