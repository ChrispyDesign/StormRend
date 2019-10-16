using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using TMPro;

namespace StormRend.UI
{
	public class BlizzardMeter : Meter
	{
		[SerializeField] Image slider;
		[SerializeField] Image[] blizzardNodes;
		[SerializeField] float amount;

		TextMeshProUGUI infoPanelText;

		private void Awake()
		{
			{
				infoPanelText = FindObjectOfType<InfoPanel>().GetComponent<TextMeshProUGUI>();
			}

			Debug.Assert(slider, "There is no slider, please add a panel with filled image component on it..");
			Debug.Assert(blizzardNodes[0], "There are no Blizzard Nodes, please add nodes");

			slider.fillAmount = 0;
		}

		public void Increase(int _iterate = 1)
		{
			for (int i = 0; i < _iterate; i++)
			{
				StartCoroutine("IncreaseBlizzard");
				OnIncrease();
			}
		}

		public void Decrease(int _iterate = 1)
		{
			for (int i = 0; i < _iterate; i++)
			{
				StartCoroutine("DecreaseBlizzard");
				OnDecrease();
			}
		}

		public override void OnIncrease()
		{
			
		}

		public override void OnDecrease()
		{
			
		}

		public override void OnPointerEnter(PointerEventData eventData)
		{
			infoPanelText.gameObject.SetActive(true);
			infoPanelText.text = details;
		}

		public override void OnPointerExit(PointerEventData eventData)
		{
			infoPanelText.gameObject.SetActive(false);
			infoPanelText.text = null;
		}

		private IEnumerator IncreaseBlizzard()
		{
			for(float i = 0f; i <= amount; i += 0.01f)
			{
				slider.fillAmount += 0.01f;
				yield return new WaitForSeconds(0.01f);
			}
			yield return null;
		}

		private IEnumerator DecreaseBlizzard()
		{
			for (float i = 0f; i <= amount; i += 0.01f)
			{
				slider.fillAmount -= 0.01f;
				yield return new WaitForSeconds(0.01f);
			}
			yield return null;
		}
	}
}