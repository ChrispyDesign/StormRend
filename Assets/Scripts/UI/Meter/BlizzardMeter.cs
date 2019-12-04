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
		void OnGUI()
		{
			if (!debug) return;

			GUILayout.Label("BlizzardMeter.internalValue: " + internalValue);
			GUILayout.Label("Blizzard (SOV): " + SOV.value);

		}

		//NOTE!
		//BhaveInt clamp and wrap function should be inbuilt into the variable. GloryController is too slow
		public override void OnChange()
		{
			if (internalValue < SOV.value)
				StartCoroutine(AutoAdjustMeter());
			else
				ResetMeter();
		}
		public void ResetMeter()		//Careful! This is called in editor
		{
			internalValue = 0; 
			foreach (var pip in meterPips)
				pip.fillAmount = 0;
		}

		// [SerializeField] BhaveInt blizzard = null;
		// [Range(0f, 1f), SerializeField] float fillSpeed = 0.03f;
		// [SerializeField] Image[] blizzardNodes = null;
		// [SerializeField] string details = null;
		
		// int internalBlizzard = 0;
		// bool increase = false;
		// bool decrease = false;
		// bool startCheck = false;

		// private void Awake()
		// {
		// 	infoPanel = FindObjectOfType<InfoPanel>();
		// 	foreach (Image img in blizzardNodes)
		// 	{
		// 		img.fillAmount = 0f;
		// 	}

		// 	Debug.Assert(blizzardNodes[0], "There is no slider, please add a panel with filled image component on it. " + typeof(BlizzardMeter));
		// 	Debug.Assert(infoPanel, string.Format("[{0}] {1} not found!", this.name, typeof(InfoPanel).Name));
		// 	Debug.Assert(blizzard, "No Blizzard SOV found!");
		// }

		// void OnEnable()
		// {
		// 	internalBlizzard = blizzard.value;
		// 	blizzard.onChanged += OnChange;

		// 	startCheck = true;
		// 	UpdatePanel();
		// }
		// void OnDestroy() => blizzard.onChanged -= OnChange;

		// //Register events
		// private void OnChange()
		// {
		// 	//Increase
		// 	if (internalBlizzard < blizzard.value)
		// 		increase = true;
		// 	//Decrease
		// 	else if (internalBlizzard > blizzard.value)
		// 		decrease = true;

		// 	internalBlizzard = blizzard.value;
		// 	UpdatePanel();
		// }

		// private void UpdatePanel()
		// {
		// 	if (increase || startCheck || Input.GetKeyDown(KeyCode.B))
		// 	{
		// 		for (int i = 0; i < blizzard.value; i++)
		// 			StartCoroutine(FillNode(i));
		// 	}

		// 	if (decrease || Input.GetKeyDown(KeyCode.C))
		// 	{
		// 		Reset();
		// 	}
		// }

		// //Coroutines
		// private IEnumerator FillNode(int _index)
		// {
		// 	if (blizzardNodes[_index].fillAmount == 1)
		// 	{
		// 		startCheck = false;
		// 		increase = false;
		// 		yield return null;
		// 	}
		// 	for (float i = 0f; i <= 1; i += fillSpeed)
		// 	{
		// 		blizzardNodes[_index].fillAmount += fillSpeed;
		// 		yield return new WaitForSeconds(fillSpeed);
		// 	}
		// }

		// private IEnumerator UnfillNode(int _index)
		// {
		// 	if (blizzardNodes[_index].fillAmount == 1)
		// 	{
		// 		startCheck = false;
		// 		decrease = false;
		// 		yield return null;
		// 	}
		// 	for (float i = 0f; i <= 1; i += fillSpeed)
		// 	{
		// 		blizzardNodes[_index].fillAmount -= fillSpeed;
		// 		yield return new WaitForSeconds(fillSpeed);
		// 	}
		// }

		// //Event System
		// public override void OnPointerEnter(PointerEventData eventData)
		// {
		// 	infoPanel?.ShowPanel(title, 1, details);
		// }

		// public override void OnPointerExit(PointerEventData eventData)
		// {
		// 	infoPanel?.UnShowPanel();
		// }
	}
}