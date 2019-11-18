using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace StormRend.UI
{
	public class InfoPanel : MonoBehaviour
	{
		[Range(0f, 2f), SerializeField] float animSpeedMultiplier = 1.05f;
		[SerializeField] GameObject[] panels = null;

		Animator anim;
		List<TextMeshProUGUI> text = new List<TextMeshProUGUI>();
		List<Image> backgrounds = new List<Image>();

		private void Awake()
		{
			anim = GetComponent<Animator>();
			foreach (GameObject panel in panels)
			{
				text.Add(panel.GetComponentInChildren<TextMeshProUGUI>());
				backgrounds.Add(panel.GetComponent<Image>());
				panel.SetActive(false);
			}
			panels[0].SetActive(true);
		}

		public void ShowPanel(string title, int levels, params string[] details)
		{
			text[0].text = title;
			for (int i = 0; i < details.Length; i++)
			{
				text[i + 1].text = details[i];
			}

			//Animate
			anim.SetFloat("SpeedMultiplier", animSpeedMultiplier);
			anim.SetInteger("textBoxAnimation", levels);
		}

		public void UnShowPanel(bool instant = false)
		{
			if (!instant)
			{
				anim.SetFloat("SpeedMultiplier", animSpeedMultiplier);
				anim.SetInteger("textBoxAnimation", 0);
			}
			else if (instant == true)
			{
				for (int i = 1; i < panels.Length; i++)
				{
					panels[i].SetActive(false);
				}
			}
		}
	}
}