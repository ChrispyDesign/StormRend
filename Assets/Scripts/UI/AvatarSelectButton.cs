using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using StormRend.Units;
using StormRend.Utility.Events;
using UnityEngine.Events;
using StormRend.Systems;
using StormRend.Variables;

namespace StormRend.UI
{
	public class AvatarSelectButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		[SerializeField] AnimateUnit unit;
		[SerializeField] string title;
		[SerializeField] string details;

		public UnitEvent onHover;
		public UnityEvent onUnHover;
		UserInputHandler inputHandler;

		InfoPanel infoPanel;

		void Awake()
		{
			infoPanel = FindObjectOfType<InfoPanel>();
			inputHandler = FindObjectOfType<UserInputHandler>();

			Debug.Assert(infoPanel, "There are no Info Panel Script in the scene. " + typeof(FinishTurn));
			Debug.Assert(infoPanel, "There are no Input Handler in the scene. " + typeof(FinishTurn));
		}

		public void ShowAbilities()
		{
			inputHandler.SelectUnit(unit);

		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			onHover.Invoke(unit);
			infoPanel.ShowPanel(title, details, 1);
		}


		public void OnPointerExit(PointerEventData eventData)
		{
			onUnHover.Invoke();
			infoPanel.UnShowPanel();
		}
	}
}