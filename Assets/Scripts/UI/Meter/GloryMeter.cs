﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using StormRend.Units;

namespace StormRend.UI
{
	public class GloryMeter : Meter
	{
		[SerializeField] List<AnimateUnit> units = new List<AnimateUnit>();
		[SerializeField] Image[] gloryNodes = null;
			
		string[] details = new string[3];
		int currentIndex;

		private void Awake()
		{
			infoPanel = FindObjectOfType<InfoPanel>();
			foreach (Image img in gloryNodes)
			{
				img.fillAmount = 0f;
			}
			currentIndex--;

			for (int i = 0; i < details.Length; i++)
			{
				details[i] = units[i].description;
			}

			Debug.Assert(gloryNodes[0], "There is no slider, please add a panel with filled image component on it. " + typeof(GloryMeter));
			Debug.Assert(infoPanel, "There are no Info Panel Script in the scene. " + typeof(GloryMeter));
		}

		public override void OnIncrease()
		{
			if (currentIndex + 1 == gloryNodes.Length)
				return;

			StartCoroutine(IncreaseBlizzard(currentIndex + 1));
			currentIndex++;
		}

		public override void OnDecrease()
		{
			if (currentIndex - 1 == -2)
				return;

			StartCoroutine(DecreaseBlizzard(currentIndex));
			currentIndex--;
		}

		public override void OnPointerEnter(PointerEventData eventData)
		{
			infoPanel.ShowPanel(title, details, 3);
		}

		public override void OnPointerExit(PointerEventData eventData)
		{
			infoPanel.UnShowPanel();
		}

		private IEnumerator IncreaseBlizzard(int _index)
		{
			for (float i = 0f; i <= 1; i += 0.03f)
			{
				gloryNodes[_index].fillAmount += 0.03f;
				yield return new WaitForSeconds(0.03f);
			}
			yield return null;
		}

		private IEnumerator DecreaseBlizzard(int _index)
		{
			for (float i = 0f; i <= 1; i += 0.03f)
			{
				gloryNodes[_index].fillAmount -= 0.03f;
				yield return new WaitForSeconds(0.03f);
			}
			yield return null;
		}
	}
}