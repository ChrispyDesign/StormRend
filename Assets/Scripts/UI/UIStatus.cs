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
        [SerializeField] AllyType allyType;
        [SerializeField] StatusType statusType;
		[SerializeField, ReadOnlyField] List<Image> icon = new List<Image>();
		[SerializeField, ReadOnlyField] AnimateUnit unit;

		InfoPanel infoPanel;
		List<GameObject> icons = new List<GameObject>();

		//Enums
		public enum AllyType
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

        private void Awake()
        {
			infoPanel = FindObjectOfType<InfoPanel>();            
        }

        //AllyUnit unit
        private void Start()
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
				case AllyType.FrostHound:
					typeToFind = typeof(FrostHoundTag);
					break;
				case AllyType.FrostTroll:
					typeToFind = typeof(FrostTrollTag);
					break;
			}
			var tag = FindObjectOfType(typeToFind) as Tag;
			unit = tag?.GetComponent<AnimateUnit>();

			for(int i = 0; i < transform.childCount; i++)
			{
				icons.Add(transform.GetChild(i).gameObject);
			}

			icon.AddRange(GetComponentsInChildren<Image>());
			CheckStatus(statusType);
		}

		public void CheckStatus(StatusType _type)
		{
			switch (_type)
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