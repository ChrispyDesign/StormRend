using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using StormRend.Units;
using pokoro.BhaVE.Core.Variables;

namespace StormRend.UI
{
	public class GloryMeter : Meter
	{
		//Inspector
		[SerializeField] BhaveInt glory = null;
		[Range(0f, 1f), SerializeField] float fillSpeed = 0.03f;
		[SerializeField] List<AnimateUnit> units = new List<AnimateUnit>();
		[SerializeField] Image[] gloryNodes = null;

		//Members
		int internalGlory = 0;
		string[] details = new string[3];

		private void Awake()
		{
			infoPanel = FindObjectOfType<InfoPanel>();
			units.AddRange(FindObjectsOfType<AllyUnit>());
			foreach (Image img in gloryNodes)
			{
				img.fillAmount = 0f;
			}
			// glory.value--;

			for (int i = 0; i < details.Length; i++)
			{
				details[i] = units[i].description;
			}

			//Init old value
			internalGlory = glory.value;

			Debug.Assert(gloryNodes[0], "There is no slider, please add a panel with filled image component on it. " + typeof(GloryMeter));
			Debug.Assert(infoPanel, "There are no Info Panel Script in the scene. " + typeof(GloryMeter));
			Debug.Assert(glory, "No Glory SOV found!");
		}

		//Register events
		// void OnEnable() => glory.onChanged += OnChange;
		// void OnDisable() => glory.onChanged -= OnChange;

		public void OnChange()
		{
			Debug.Log("OnChange");
			//Increase
			if (internalGlory < glory.value)
				StartCoroutine(IncreaseGlory(glory.value - 1));
			//Decrease
			else if (internalGlory > glory.value)
				StartCoroutine(DecreaseGlory(glory.value + 1));
		}

		public override void OnPointerEnter(PointerEventData eventData)
		{
			infoPanel.ShowPanel(title, 3, details);
		}

		public override void OnPointerExit(PointerEventData eventData)
		{
			infoPanel.UnShowPanel();
		}

		IEnumerator IncreaseGlory(int _index)
		{
			// index--;
            for (float i = 0f; i <= 1; i += fillSpeed)
			{
                gloryNodes[_index].fillAmount += fillSpeed;
				yield return new WaitForSeconds(fillSpeed);
			}
			yield return null;
		}

		IEnumerator DecreaseGlory(int _index)
		{
            for (float i = 0f; i <= 1; i += fillSpeed)
			{
                gloryNodes[_index].fillAmount -= fillSpeed;
				yield return new WaitForSeconds(fillSpeed);
			}
			yield return null;
		}
	}
}