using UnityEngine;
using UnityEngine.EventSystems;

namespace StormRend.UI
{ 
	public class Meter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		[SerializeField] protected string details;

		public virtual void OnIncrease()
		{

		}

		public virtual void OnDecrease()
		{

		}

		public virtual void OnPointerEnter(PointerEventData eventData)
		{
			throw new System.NotImplementedException();
		}

		public virtual void OnPointerExit(PointerEventData eventData)
		{
			throw new System.NotImplementedException();
		}
	}
}
