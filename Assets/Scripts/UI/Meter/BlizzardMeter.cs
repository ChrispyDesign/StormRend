using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using TMPro;

namespace StormRend.UI
{
	public class BlizzardMeter : Meter
	{
		[SerializeField] Image[] blizzardNodes;
		int currentIndex;

		private void Awake()
		{
			infoPanel = FindObjectOfType<InfoPanel>();
			foreach (Image img in blizzardNodes)
			{
				img.fillAmount = 0f;
			}
			currentIndex--;

			Debug.Assert(blizzardNodes[0], "There is no slider, please add a panel with filled image component on it. " + typeof(BlizzardMeter));
			Debug.Assert(infoPanel, "There are no Info Panel Script in the scene. " + typeof(BlizzardMeter));
		}

		public override void OnIncrease()
		{
			if (currentIndex + 1 == blizzardNodes.Length)
				return;

			StartCoroutine(IncreaseBlizzard(currentIndex + 1));
			currentIndex++;
		}

		public override void OnDecrease()
		{
			if (currentIndex - 1 == -1)
				return;

			StartCoroutine(DecreaseBlizzard(currentIndex));
			currentIndex--;
		}

		public override void OnPointerEnter(PointerEventData eventData)
		{
			infoPanel.ShowPanel(details);
		}

		public override void OnPointerExit(PointerEventData eventData)
		{
			infoPanel.UnShowPanel();
		}

		private IEnumerator IncreaseBlizzard(int _index)
		{
			for(float i = 0f; i <= 1; i += 0.1f)
			{
				blizzardNodes[_index].fillAmount += 0.1f;
				yield return new WaitForSeconds(0.1f);
			}
			yield return null;
		}

		private IEnumerator DecreaseBlizzard(int _index)
		{
			for (float i = 0f; i <= 1; i += 0.1f)
			{
				blizzardNodes[_index].fillAmount -= 0.1f;
				yield return new WaitForSeconds(0.1f);
			}
			yield return null;
		}
	}
}