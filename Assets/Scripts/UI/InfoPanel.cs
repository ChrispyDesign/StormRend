using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace StormRend.UI
{
	public class InfoPanel : MonoBehaviour
	{
		[Range(0, 1)]
		[SerializeField] float fadeDuration;
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

		public void ShowPanel(string title, string details, int levels)
		{
			text[0].text = title;
			text[1].text = details;
			anim.SetInteger("textBoxAnimation", levels);
		}

		public void ShowPanel(string title, string[] details, int levels)
		{
			text[0].text = title;
			for(int i = 0; i < details.Length; i++)
			{
				text[i + 1].text = details[i];
			}
			anim.SetInteger("textBoxAnimation", levels);
		}

		public void UnShowPanel()
		{
			anim.SetInteger("textBoxAnimation", 0);
		}
	}
}