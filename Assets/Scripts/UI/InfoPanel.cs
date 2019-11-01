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
		[SerializeField] GameObject[] panels;

		List<TextMeshProUGUI> text = new List<TextMeshProUGUI>();
		List<Image> backgrounds = new List<Image>();

		private void Awake()
		{
			foreach(GameObject gameobject in panels)
			{
				text.Add(gameObject.GetComponentInChildren<TextMeshProUGUI>());
				backgrounds.Add(gameObject.GetComponent<Image>());
			}
		}

		public void ShowPanel(string title, string details, int levels)
		{
			for(int i = )
			StopAllCoroutines();
			StartCoroutine(FadeIn());
		}

		public void UnShowPanel()
		{
			StopAllCoroutines();
			StartCoroutine(FadeOut());
		}

		IEnumerator FadeIn()
		{
			for (float i = 0f; i <= 1f; i += fadeDuration)
			{
				Vector4 color;
				{
					//color = background.color;
					//color.w = i;
					//background.color = color;
				}

				{
					//color = text.color;
					color.w = i;
					//text.color = color;
				}
				yield return new WaitForSeconds(0.01f);
			}
			yield return null;
		}

		IEnumerator FadeOut()
		{
			for (float i = 1f; i >= -0.05f; i -= fadeDuration)
			{
				Vector4 color;
				{
					//color = background.color;
					//color.w = i;
					//background.color = color;
				}

				{
					//color = text.color;
					color.w = i;
					//text.color = color;
				}
				yield return new WaitForSeconds(0.01f);
			}
			yield return null;
		}
	}
}