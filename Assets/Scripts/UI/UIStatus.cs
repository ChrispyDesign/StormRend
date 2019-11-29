using StormRend.Abilities;
using StormRend.Tags;
using StormRend.UI;
using StormRend.Units;
using StormRend.Utility.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace StormRend.UI
{
	public class UIStatus : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		[SerializeField] new string name;
		[SerializeField] string details;
        [SerializeField] UnitType allyType;
        [SerializeField] StatusType statusType;
		[SerializeField] AnimateUnit unit;

		[SerializeField, ReadOnlyField] List<Image> icon = new List<Image>();

		InfoPanel infoPanel;

		//Enums
		public enum UnitType
		{
			Off,
			Berserker,
			Valkyrie,
			Sage,
			FrostHound,
			FrostTroll
		}

        public enum StatusType
        {
            Off,
            Protection,
            Immobilised,
            Blinded
        }
		private void OnDestroy()
		{
			unit?.onAddStatusEffect.RemoveListener(CheckStatus);			
		}

		private void Awake()
        {
			infoPanel = FindObjectOfType<InfoPanel>();
			icon.AddRange(GetComponentsInChildren<Image>());

			Type typeToFind = null;
			bool isEnemy = false;
			switch (allyType)
			{
				case UnitType.Berserker:
					typeToFind = typeof(BerserkerTag);
					break;
				case UnitType.Valkyrie:
					typeToFind = typeof(ValkyrieTag);
					break;
				case UnitType.Sage:
					typeToFind = typeof(SageTag);
					break;
				case UnitType.FrostHound:
					typeToFind = typeof(FrostHoundTag);
					isEnemy = true;
					break;
				case UnitType.FrostTroll:
					typeToFind = typeof(FrostTrollTag);
					isEnemy = true;
					break;
			}

			var tag = FindObjectOfType(typeToFind) as Tag;
			if (!isEnemy)
				unit = tag?.GetComponent<AnimateUnit>();
			else
				CheckStatus();

			RegisterEvents();
		}

		void RegisterEvents()
		{
			if (!unit) return;

			unit.onAddStatusEffect.AddListener(CheckStatus);
			unit.onBeginTurn.AddListener(CheckStatus);
		}

		void CheckStatus() => CheckStatus(null);	//Relay
		void CheckStatus(Effect effect = null)
		{
			switch (statusType)
			{
				case StatusType.Protection:
					TurnIconOnAndOff(unit.isProtected);
					break;
				case StatusType.Immobilised:
					TurnIconOnAndOff(unit.isImmobilised);
					break;
				case StatusType.Blinded:
					TurnIconOnAndOff(unit.isBlind);
					break;
			}
		}

		void TurnIconOnAndOff(bool _isOn)
		{
			if (!_isOn)
			{
				foreach (Image img in icon)
				{
					img.fillAmount = 0;
				}
			}
			else
			{
				foreach (Image img in icon)
				{
					img.fillAmount = 1;
				}
			}
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			if(icon[0].fillAmount >= 0.5f)
				infoPanel?.ShowPanel(name, 1, details);
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			infoPanel?.UnShowPanel();
		}
	}
}