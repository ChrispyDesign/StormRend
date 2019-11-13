using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using StormRend.Abilities;
using System.Collections.Generic;
using StormRend.Utility.Events;
using UnityEngine.Events;

namespace StormRend.UI
{
	public class AbilityDetails : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		[SerializeField] Ability ability = null;
		[SerializeField] Image icon = null;
		[SerializeField] AbilityEvent onHover = null;
		[SerializeField] AbilityEvent onClick = null;
		[SerializeField] UnityEvent onUnHover = null;
		InfoPanel infoPanel;
		Button button;

		public void SetAbility(Ability _ability)
		{
			ability = _ability;
			icon.sprite = _ability.icon;
		}

		void Awake()
		{
			button = GetComponent<Button>();
			infoPanel = FindObjectOfType<InfoPanel>();
			Debug.Assert(infoPanel, "There are no Info Panel Script in the scene. " + typeof(FinishTurn));
		}
		
		//Register
		void OnEnable() => button.onClick.AddListener(OnClick);
		void OnDisable() => button.onClick.RemoveAllListeners();

		//Onhover
		public void OnPointerEnter(PointerEventData eventData)
		{
			onHover.Invoke(ability);

			if (ability != null)
			{
				string[] details = new string[ability.level];

				for(int i = 0; i < ability.level; i++)
				{
					details[i] = ability.descriptions[i];
				}

				infoPanel.ShowPanel(ability.title, ability.level, details);
			}
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			onUnHover.Invoke();
			infoPanel.UnShowPanel();
		}

		void OnClick() => onClick.Invoke(ability);
	}
}