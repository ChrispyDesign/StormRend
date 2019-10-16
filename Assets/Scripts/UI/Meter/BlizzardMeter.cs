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
		GameObject infoPanelParent;

		private void Awake()
		{
			{
				infoPanelText = FindObjectOfType<InfoPanel>().GetComponent<TextMeshProUGUI>();
				infoPanelParent = infoPanelText.transform.parent.gameObject;

				infoPanelParent.SetActive(false);
				slider.fillAmount = 0;
			}

			Debug.Assert(slider, "There is no slider, please add a panel with filled image component on it..");
			Debug.Assert(blizzardNodes[0], "There are no Blizzard Nodes, please add nodes");
			Debug.Assert(infoPanelText, "There are no Text Mesh Pro Component in Info Panel");
			Debug.Assert(infoPanelParent, "There are no Info Panel Parent");
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
			infoPanelParent.SetActive(true);
			infoPanelText.text = details;
		}

		public override void OnPointerExit(PointerEventData eventData)
		{
			infoPanelParent.SetActive(false);
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