using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using TMPro;

namespace StormRend.UI
{
	public class BlizzardMeter : Meter
	{
		[SerializeField] float increasePerNode;
		[SerializeField] Image slider;
		[SerializeField] Image[] blizzardNodes;

		private void Awake()
		{
			infoPanel = FindObjectOfType<InfoPanel>();
			slider.fillAmount = 0f;		

			Debug.Assert(slider, "There is no slider, please add a panel with filled image component on it. " + typeof(BlizzardMeter));
			Debug.Assert(blizzardNodes[0], "There are no Blizzard Nodes, please add nodes. " + typeof(BlizzardMeter));
			Debug.Assert(infoPanel, "There are no Info Panel Script in the scene. " + typeof(BlizzardMeter));
		}

		public override void OnIncrease()
		{
			StartCoroutine(IncreaseBlizzard());
		}

		public override void OnDecrease()
		{
			StartCoroutine(DecreaseBlizzard());
		}

		public override void OnPointerEnter(PointerEventData eventData)
		{
			infoPanel.ShowPanel(details);
		}

		public override void OnPointerExit(PointerEventData eventData)
		{
			infoPanel.UnShowPanel();
		}

		private IEnumerator IncreaseBlizzard()
		{
			for(float i = 0f; i <= increasePerNode; i += 0.01f)
			{
				slider.fillAmount += 0.01f;
				yield return new WaitForSeconds(0.01f);
			}
			yield return null;
		}

		private IEnumerator DecreaseBlizzard()
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