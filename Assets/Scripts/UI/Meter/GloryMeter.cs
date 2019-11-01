using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

namespace StormRend.UI
{
	public class GloryMeter : Meter
	{
		[SerializeField] Image[] gloryNodes;
		int currentIndex;

		private void Awake()
		{
			infoPanel = FindObjectOfType<InfoPanel>();
			foreach (Image img in gloryNodes)
			{
				img.fillAmount = 0f;
			}
			currentIndex--;

			Debug.Assert(gloryNodes[0], "There is no slider, please add a panel with filled image component on it. " + typeof(GloryMeter));
			Debug.Assert(infoPanel, "There are no Info Panel Script in the scene. " + typeof(GloryMeter));
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.B))
				OnIncrease();

			if (Input.GetKeyDown(KeyCode.N))
				OnDecrease();
		}

		public override void OnIncrease()
		{
			if (currentIndex + 1 == gloryNodes.Length)
				return;

			StartCoroutine(IncreaseBlizzard(currentIndex + 1));
			currentIndex++;
		}

		public override void OnDecrease()
		{
			if (currentIndex - 1 == -2)
				return;

			StartCoroutine(DecreaseBlizzard(currentIndex));
			currentIndex--;
		}

		public override void OnPointerEnter(PointerEventData eventData)
		{
			infoPanel.ShowPanel(title, details, 1);
		}

		public override void OnPointerExit(PointerEventData eventData)
		{
			infoPanel.UnShowPanel();
		}

		private IEnumerator IncreaseBlizzard(int _index)
		{
			for (float i = 0f; i <= 1; i += 0.01f)
			{
				gloryNodes[_index].fillAmount += 0.01f;
				yield return new WaitForSeconds(0.01f);
			}
			yield return null;
		}

		private IEnumerator DecreaseBlizzard(int _index)
		{
			for (float i = 0f; i <= 1; i += 0.01f)
			{
				gloryNodes[_index].fillAmount -= 0.01f;
				yield return new WaitForSeconds(0.01f);
			}
			yield return null;
		}
	}
}