using StormRend.Abilities;
using StormRend.Tags;
using StormRend.UI;
using StormRend.Units;
using StormRend.Utility.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace StormRend.UI
{
	public class UIStatus : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		[SerializeField] string name;
		[SerializeField] string details;
        [SerializeField] UnitType allyType;
        [SerializeField] StatusType statusType;
		[SerializeField, ReadOnlyField] List<Image> icon = new List<Image>();
		[SerializeField, ReadOnlyField] AnimateUnit unit;

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
			unit.onAddStatusEffect.RemoveListener(CheckStatus);			
		}

		private void Awake()
        {
			infoPanel = FindObjectOfType<InfoPanel>();
			icon.AddRange(GetComponentsInChildren<Image>());

			Type typeToFind = null;
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
					break;
				case UnitType.FrostTroll:
					typeToFind = typeof(FrostTrollTag);
					break;
			}
			var tag = FindObjectOfType(typeToFind) as Tag;
			unit = tag?.GetComponent<AnimateUnit>();

			unit.onAddStatusEffect.AddListener(CheckStatus);
			unit.onBeginTurn.AddListener(CheckStatus);
		}
		private void Update()
		{
			switch (statusType)
			{
				case StatusType.Protection:
					if(unit.isProtected)
						Debug.LogWarning(unit.name + " Protection " + unit.isProtected);
					break;
				case StatusType.Immobilised:
					if (unit.isImmobilised)
						Debug.LogWarning(unit.name + " Immobilised " + unit.isImmobilised);
					break;
				case StatusType.Blinded:
					if (unit.isBlind)
						Debug.LogWarning(unit.name + " Blinded " + unit.isBlind);
					break;
			}
		}

		void CheckStatus() => CheckStatus(null);

		void CheckStatus(Effect effect)
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
			infoPanel?.ShowPanel(name, 1, details);
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			infoPanel?.UnShowPanel();
		}
	}
}