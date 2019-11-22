using StormRend.Tags;
using StormRend.UI;
using StormRend.Units;
using StormRend.Utility.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Stormrend.UI
{
	public class UnitUIStatus : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		[SerializeField] string details;
		[ReadOnlyField] AllyType allyType;
		[ReadOnlyField] AnimateUnit unit;

		InfoPanel infoPanel;
		List<GameObject> icons = new List<GameObject>();

		//Enums
		public enum AllyType
		{
			Off,
			Berserker,
			Valkyrie,
			Sage
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
			}
			var tag = FindObjectOfType(typeToFind) as Tag;
			unit = tag?.GetComponent<AnimateUnit>();
			infoPanel = FindObjectOfType<InfoPanel>();

			for(int i = 0; i < transform.childCount; i++)
			{
				icons.Add(transform.GetChild(i).gameObject);
			}
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			infoPanel?.ShowPanel(gameObject.name, 1, details);
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			infoPanel?.UnShowPanel();
		}
	}
}