using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using StormRend.Abilities;
using StormRend.Utility.Events;
using UnityEngine.Events;
using StormRend.Utility.Attributes;
using StormRend.Assists;

namespace StormRend.UI
{
	public class AbilityDetails : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		//Inspector
		[ReadOnlyField, SerializeField] Ability ability = null;
		[ReadOnlyField, SerializeField] Image icon = null;
		[SerializeField] AbilityEvent onHover = null;
		[SerializeField] AbilityEvent onClick = null;
		[SerializeField] UnityEvent onUnHover = null;

		//Members
		InfoPanel infoPanel;
		Button button;

		public void SetAbility(Ability _ability)
		{
			ability = _ability;
			if (ability) icon.sprite = _ability.icon;
		}

		void Awake()
		{
			button = GetComponent<Button>();
			infoPanel = FindObjectOfType<InfoPanel>();

			Debug.Assert(infoPanel, string.Format("[{0}] {1} not found!", this.name, typeof(InfoPanel).Name));
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

				for (int i = 0; i < ability.level; i++)
				{
					details[i] = ability.descriptions[i];
				}

				infoPanel?.ShowPanel(ability.title, ability.level, details);
			}
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			onUnHover.Invoke();
			infoPanel?.UnShowPanel();
		}

		void OnClick() => onClick.Invoke(ability);
	}
}