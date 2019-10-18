using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

namespace StormRend.UI
{
	public class GloryMeter : Meter
	{
		[SerializeField] float increasePerNode;
		[SerializeField] Image slider;

		private void Awake()
		{
			infoPanel = FindObjectOfType<InfoPanel>();
			slider.fillAmount = 0f;

			Debug.Assert(slider, "There is no slider, please add a panel with filled image component on it. " + typeof(GloryMeter));
			Debug.Assert(infoPanel, "There are no Info Panel Script in the scene. " + typeof(GloryMeter));
		}
		
		public override void OnIncrease()
		{
			StartCoroutine(IncreaseGlory());
		}

		public override void OnDecrease()
		{
			StartCoroutine(DecreaseGlory());
		}

		public override void OnPointerEnter(PointerEventData eventData)
		{
			infoPanel.ShowPanel(details);
		}

		public override void OnPointerExit(PointerEventData eventData)
		{
			infoPanel.UnShowPanel();
		}

		private IEnumerator IncreaseGlory()
		{
			for (float i = 0f; i <= increasePerNode; i += 0.01f)
			{
				slider.fillAmount += 0.01f;
				yield return new WaitForSeconds(0.01f);
			}
			yield return null;
		}

		private IEnumerator DecreaseGlory()
		{
			for (float i = 0f; i <= increasePerNode; i += 0.01f)
			{
				slider.fillAmount -= 0.01f;
				yield return new WaitForSeconds(0.01f);
			}
			yield return null;
		}
	}
}