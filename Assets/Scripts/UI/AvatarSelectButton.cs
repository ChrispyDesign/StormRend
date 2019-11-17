using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using StormRend.Units;
using StormRend.Utility.Events;
using UnityEngine.Events;
using StormRend.Systems;
using StormRend.Assists;

namespace StormRend.UI
{
	public class AvatarSelectButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		[SerializeField] AnimateUnit unit = null;
		[SerializeField] List<Image> healthNodes = new List<Image>();

		string title;
		string details;
		public UnitEvent onHover;
		public UnityEvent onUnHover;
		UserInputHandler inputHandler;

		InfoPanel infoPanel;

		void Awake()
		{
			infoPanel = FindObjectOfType<InfoPanel>();
			inputHandler = FindObjectOfType<UserInputHandler>();

			Debug.Assert(infoPanel, "There are no Info Panel Script in the scene. " + typeof(UnusedActionsChecker));
			Debug.Assert(infoPanel, "There are no Input Handler in the scene. " + typeof(UnusedActionsChecker));
		}

		public void ShowAbilities()
		{
			inputHandler.SelectUnit(unit);
		}

		public void UpdateGUIHealthBar()
		{
			foreach (Image img in healthNodes)
			{
				img.fillAmount = 0;
			}

			for (int i = 0; i < unit.HP; i++)
			{
				healthNodes[i].fillAmount = 1;
			}
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			if (unit)	//Null checks
			{
				onHover.Invoke(unit);
				infoPanel.ShowPanel(unit.name, 1, unit.description);
			}
		}


		public void OnPointerExit(PointerEventData eventData)
		{
			onUnHover.Invoke();
			infoPanel.UnShowPanel();
		}
	}
}