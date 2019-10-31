using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace StormRend.UI
{
	public class InfoPanel : MonoBehaviour
	{
		[Range(0, 1)]
		[SerializeField] float fadeDuration;
		[SerializeField] TextMeshProUGUI text;
		[SerializeField] Image background;
		[SerializeField] GameObject parent;

		private void Awake()
		{
			Vector4 color = background.color;
			color.w = 0;
			background.color = color;
		}

		public void ShowPanel(string details)
		{
			text.text = details;
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
					color = background.color;
					color.w = i;
					background.color = color;
				}

				{
					color = text.color;
					color.w = i;
					text.color = color;
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
					color = background.color;
					color.w = i;
					background.color = color;
				}

				{
					color = text.color;
					color.w = i;
					text.color = color;
				}
				yield return new WaitForSeconds(0.01f);
			}
			yield return null;
		}
	}
}