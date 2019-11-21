using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using pokoro.BhaVE.Core.Variables;
using System;

namespace StormRend.UI
{
    public class BlizzardMeter : Meter
	{
		[SerializeField] BhaveInt blizzard = null;
		[Range(0f, 1f), SerializeField] float fillSpeed = 0.03f;
		[SerializeField] Image[] blizzardNodes = null;
		[SerializeField] string details = null;
		
		int internalBlizzard = 0;
		bool increase = false;
		bool decrease = false;
		bool startCheck = false;

		private void Awake()
		{
			infoPanel = FindObjectOfType<InfoPanel>();
			foreach (Image img in blizzardNodes)
			{
				img.fillAmount = 0f;
			}
			internalBlizzard = blizzard.value;

			Debug.Assert(blizzardNodes[0], "There is no slider, please add a panel with filled image component on it. " + typeof(BlizzardMeter));
			Debug.Assert(infoPanel, string.Format("[{0}] {1} not found!", this.name, typeof(InfoPanel).Name));
			Debug.Assert(blizzard, "No Blizzard SOV found!");

			startCheck = true;
			UpdatePanel();
		}

		void OnEnable() => blizzard.onChanged += OnChange;
		void OnDisable() => blizzard.onChanged -= OnChange;

		private void Update()
		{
			UpdatePanel();
		}
		
		//Register events
		private void OnChange()
		{
			//Increase
			if (internalBlizzard < blizzard.value)
				increase = true;
			//Decrease
			else if (internalBlizzard > blizzard.value)
				decrease = true;

			internalBlizzard = blizzard.value;
		}

		private void UpdatePanel()
		{
			if (increase || startCheck)
			{
				for (int i = 0; i < blizzard.value; i++)
					StartCoroutine(FillNode(i));
			}

			if (decrease)
			{
				for (int i = blizzardNodes.Length - 1; i >= blizzard.value; i--)
					StartCoroutine(UnfillNode(i));
			}
		}

		public void Reset()
		{
			foreach (var n in blizzardNodes)
			{
				n.fillAmount = 0;
			}
		}

		//Coroutines
		private IEnumerator FillNode(int _index)
		{
			if (blizzardNodes[_index].fillAmount == 1)
			{
				startCheck = false;
				increase = false;
				yield return null;
			}
			for (float i = 0f; i <= 1; i += fillSpeed)
			{
				blizzardNodes[_index].fillAmount += fillSpeed;
				yield return new WaitForSeconds(fillSpeed);
			}
		}

		private IEnumerator UnfillNode(int _index)
		{
			if (blizzardNodes[_index].fillAmount == 1)
			{
				startCheck = false;
				decrease = false;
				yield return null;
			}
			for (float i = 0f; i <= 1; i += fillSpeed)
			{
				blizzardNodes[_index].fillAmount -= fillSpeed;
				yield return new WaitForSeconds(fillSpeed);
			}
		}

		//Event System
		public override void OnPointerEnter(PointerEventData eventData)
		{
			infoPanel?.ShowPanel(title, 1, details);
		}

		public override void OnPointerExit(PointerEventData eventData)
		{
			infoPanel?.UnShowPanel();
		}
	}
}