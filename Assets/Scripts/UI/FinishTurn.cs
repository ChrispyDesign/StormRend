using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

namespace StormRend.UI
{
	public class FinishTurn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		[SerializeField] string details;

		InfoPanel infoPanel;

		void Awake()
		{
			infoPanel = FindObjectOfType<InfoPanel>();
			Debug.Assert(infoPanel, "There are no Info Panel Script in the scene. " + typeof(FinishTurn));
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			infoPanel.ShowPanel(details);
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			infoPanel.UnShowPanel();
		}
	}
}