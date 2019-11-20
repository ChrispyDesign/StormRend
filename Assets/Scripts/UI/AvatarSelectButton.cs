using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using StormRend.Units;
using StormRend.Utility.Events;
using UnityEngine.Events;
using StormRend.Systems;
using StormRend.Assists;
using StormRend.Tags;
using System;
using StormRend.Utility.Attributes;

namespace StormRend.UI
{
	[RequireComponent(typeof(Button))]
	public class AvatarSelectButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		//Enums
		public enum AllyType
		{
			Off,
			Berserker,
			Valkyrie,
			Sage
		}

		//Inspector
		[SerializeField] AllyType allyType = AllyType.Berserker;
		[ReadOnlyField, SerializeField] AnimateUnit unit = null;
		[SerializeField] List<Image> healthNodes = new List<Image>();

		//Events
		public UnitEvent onHover = null;
		public UnityEvent onUnhover = null;

		//Members
		string title = null;
		string details = null;
		UserInputHandler userInputHandler = null;
		InfoPanel infoPanel = null;
		Button button = null;

		//Core
		void Awake()
		{
			infoPanel = FindObjectOfType<InfoPanel>();
			userInputHandler = FindObjectOfType<UserInputHandler>();
			button = GetComponent<Button>();

			Debug.Assert(infoPanel, string.Format("[{0}] {1} not found!", this.name, typeof(InfoPanel).Name));
			Debug.Assert(userInputHandler, string.Format("[{0}] {1} not found!", this.name, typeof(UserInputHandler).Name));

			AutoLocateUnit();
			if (!unit)
			{
				Debug.LogWarningFormat("[{0}] Unit not found! Shutting down...", this.name);
				gameObject.SetActive(false);
			}
		}
		void OnEnable()
		{
			//Hook up button
			button.onClick.AddListener(SelectUnit);
			unit?.onHeal.AddListener(UpdateHealthGUI);
			unit?.onTakeDamage.AddListener(UpdateHealthGUI);
			unit?.onDeath.AddListener(RelayUpdateHealthGUI);
		}
		void OnDisable()
		{
			button.onClick.RemoveListener(SelectUnit);
			unit?.onHeal.RemoveListener(UpdateHealthGUI);
			unit?.onTakeDamage.RemoveListener(UpdateHealthGUI);
			unit?.onDeath.AddListener(RelayUpdateHealthGUI);
		}

		void AutoLocateUnit()
		{
			Type typeToFind = null;
			switch (allyType)
			{
				case AllyType.Berserker:
					typeToFind = typeof(BerserkerTag);
					break;
				case AllyType.Valkyrie:
					typeToFind = typeof(ValkyrieTag);
					break;
				case AllyType.Sage:
					typeToFind = typeof(SageTag);
					break;
			}
			var tag = FindObjectOfType(typeToFind) as Tag;
			unit = tag?.GetComponent<AnimateUnit>();
		}

		/// <summary>
		/// On click
		/// </summary>
		public void SelectUnit() => userInputHandler.SelectUnit(unit, true);

		public void RelayUpdateHealthGUI(Unit unit) => UpdateHealthGUI(new HealthData());
		public void UpdateHealthGUI(HealthData data)
		{
			//Clear
			foreach (var n in healthNodes)
				n.fillAmount = 0f;
			//Fill
			for (int i = 0; i < unit.HP; i++)
				healthNodes[i].fillAmount = 1f;
		}

		//Event System Callbacks
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (unit)	//Null checks
			{
				onHover.Invoke(unit);
				infoPanel?.ShowPanel(unit.name, 1, unit.description);
			}
		}
		public void OnPointerExit(PointerEventData eventData)
		{
			onUnhover.Invoke();
			infoPanel?.UnShowPanel();
		}
	}
}