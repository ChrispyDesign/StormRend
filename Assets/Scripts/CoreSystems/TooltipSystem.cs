using pokoro.Patterns.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StormRend.UI
{
	public sealed class TooltipSystem : Singleton<TooltipSystem>
	{
		//Inspector
		[Tooltip("Leave empty if using screen space overlay")]
		[SerializeField] Camera UICamera = null;
		public float delay = 1.5f;
		[SerializeField] float fontSize = 20f;
		[SerializeField] TextAlignmentOptions alignment = TextAlignmentOptions.CenterGeoAligned;
		[SerializeField] float padding = 5f;

		//Properties
		public float preferredTextWidth => tooltipText.preferredWidth + padding * 2f;
		public float preferredTextHeight => tooltipText.preferredHeight + padding * 2f;

		//Members
		Image tooltipPanel = null;
		TextMeshProUGUI tooltipText = null;
		RectTransform rectTransform = null;
		RectTransform canvasRectTransform = null;

		//Inits
		void Awake()
		{
			//Grab panel and text box
			tooltipPanel = GetComponentInChildren<Image>();
			tooltipText = GetComponentInChildren<TextMeshProUGUI>();

			//Assert
			Debug.Assert(tooltipPanel, "No panel found!");
			Debug.Assert(tooltipText, "No text found!");

			//Hide from view
			Hide();
		}

		void Start()
		{
			//Get transforms
			// rectTransform = tooltipPanel.rectTransform;
			rectTransform = GetComponent<RectTransform>();
			canvasRectTransform = transform.parent.GetComponent<RectTransform>();
		}

		//Statics
		public static void Show(string message) => current.InternalShow(message);
		public static void Hide() => current.InternalHide();

		//Core
		void Update()
		{
			RectTransformUtility.
				ScreenPointToLocalPointInRectangle
					(canvasRectTransform, Input.mousePosition, UICamera, out Vector2 mouseScreenPoint);

			Vector2 finalPoint = mouseScreenPoint;

			//Offset in case the tooltip is too far to the right
			//Align left if in the right half of the screen
			if (mouseScreenPoint.x > 0) finalPoint.x = mouseScreenPoint.x - preferredTextWidth;

			//Align bottom if in the top half of the screen
			if (mouseScreenPoint.y > 0) finalPoint.y = mouseScreenPoint.y - preferredTextHeight;

			transform.localPosition = finalPoint;
		}

		void InternalShow(string message)
		{
			tooltipPanel.gameObject.SetActive(true);
			// var origTextColor = tooltipText.color;
			// var origPanelColor = tooltipPanel.color;
			// tooltipText.color = Color.clear;
			// tooltipPanel.color = Color.clear;

			//Set text
			tooltipText.text = message;
			tooltipText.fontSize = fontSize;
			tooltipText.alignment = alignment;

			//Set position
			rectTransform.sizeDelta = new Vector2(preferredTextWidth, preferredTextHeight);

			//Hopefully activating after everything is set eliminates the visual glitch
			// tooltipText.color = origTextColor;
			// tooltipPanel.color = origPanelColor;
		}
		void InternalHide()
		{
			tooltipPanel.gameObject.SetActive(false);
		}
	}
}