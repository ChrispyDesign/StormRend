using UnityEngine;
using StormRend.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace StormRend.Assists
{
	/// <summary>
	/// This is mainly required because it's animated
	/// </summary>
	[RequireComponent(typeof(Animator), typeof(AudioSource))]
	public class EndTurnButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
	{
		//Inspector
		[Header("SFX")]
		[SerializeField] AudioClip onClick = null;
		[SerializeField] AudioClip onHover = null;
		[SerializeField] AudioClip onUnhover = null;

		public UnityEvent OnClick = null;

		//Members
		Animator anim = null;
		AudioSource audSrc = null;

		void Awake()
		{
			anim = GetComponent<Animator>();
			audSrc = GetComponent<AudioSource>();
		}

		//Event system callbacks
		public void OnPointerEnter(PointerEventData eventData)
		{
			anim.SetBool("OnHover", true);
			audSrc.PlayOneShot(onHover);
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			anim.SetBool("OnHover", false);
			audSrc.PlayOneShot(onUnhover);
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			anim.SetTrigger("OnClick");
			audSrc.PlayOneShot(onClick);
			OnClick.Invoke();
		}
	}
}