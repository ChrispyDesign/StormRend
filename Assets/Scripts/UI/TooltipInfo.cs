using UnityEngine;
using UnityEngine.EventSystems;

namespace StormRend.UI
{
	public class TooltipInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
	{
		//Inspector
		[TextArea(4, 12), SerializeField] string _message = null;
		[SerializeField] protected float delayOverride = 0f;

		//Properties
		protected virtual string message => _message;

		//Members
		TooltipSystem tts = null;
		bool isHovering = false;
		float time = 0;

		protected virtual void Awake() => tts = TooltipSystem.current;

		void Update()
		{
			if (message == null && message?.Length <= 0) return;    //Don't bother if there's nothing to display

			if (!isHovering) return;


			//Count up time
			time += Time.deltaTime;

			//Determine final delay
			float finalDelay = (delayOverride > 0) ? delayOverride : tts.delay;

			//If hovered long enough then display the tooltip
			if (time > finalDelay)
				TooltipSystem.Show(message);
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			//Send 
			if (!isHovering)
			{
				isHovering = true;
				time = 0;
			}
		}

		//Prevent overly persistent tooltips over buttons that dissapear
		public void OnPointerDown(PointerEventData eventData) => OnPointerExit(eventData);

		public void OnPointerExit(PointerEventData eventData)
		{
			isHovering = false;
			TooltipSystem.Hide();
		}

	}
}