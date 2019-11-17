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
		bool increase;
		bool decrease;
		bool startCheck;

		private void Awake()
		{
			infoPanel = FindObjectOfType<InfoPanel>();
			units.AddRange(FindObjectsOfType<AllyUnit>());
			foreach (Image img in gloryNodes)
			{
				img.fillAmount = 0f;
			}

			for (int i = 0; i < units.Count; i++)
			{
				details[i] = units[i].description;
			}

			//Init old value
			internalGlory = glory.value;

			Debug.Assert(gloryNodes[0], "There is no slider, please add a panel with filled image component on it. " + typeof(GloryMeter));
			Debug.Assert(infoPanel, "There are no Info Panel Script in the scene. " + typeof(GloryMeter));
			Debug.Assert(glory, "No Glory SOV found!");

			startCheck = true;
			UpdatePanel();
		}

		//Register events
		void OnEnable() => glory.onChanged += OnChange;
		void OnDisable() => glory.onChanged -= OnChange;

		private void Update()
		{
			UpdatePanel();
		}

		public void OnChange()
		{
			//Increase
			if (internalGlory < glory.value)
				increase = true;
			//Decrease
			else if (internalGlory > glory.value)
				decrease = true;
		}

		public void UpdatePanel()
		{
			if (increase || startCheck)
			{
				for (int i = 0; i < glory.value; i++)
					StartCoroutine(IncreaseGlory(i));
			}

			if (decrease)
			{
				for (int i = gloryNodes.Length - 1; i >= glory.value; i--)
					StartCoroutine(DecreaseGlory(i));
			}
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
			if(gloryNodes[_index].fillAmount == 1)
			{
				startCheck = false;
				increase = false;
				yield return null;
			}
            for (float i = 0f; i <= 1; i += fillSpeed)
			{
                gloryNodes[_index].fillAmount += fillSpeed;
				yield return new WaitForSeconds(fillSpeed);
			}
		}

		IEnumerator DecreaseGlory(int _index)
		{
			if (gloryNodes[_index].fillAmount == 1)
			{
				startCheck = false;
				decrease = false;
				yield return null;
			}
			for (float i = 0f; i <= 1; i += fillSpeed)
			{
				gloryNodes[_index].fillAmount -= fillSpeed;
				yield return new WaitForSeconds(fillSpeed);
			}
		}
	}
}