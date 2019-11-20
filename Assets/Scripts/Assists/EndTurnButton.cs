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
		[SerializeField] string title = "End Turn";

		[Header("SFX")]
		[SerializeField] AudioClip onClick = null;
		[SerializeField] AudioClip onHover = null;
		[SerializeField] AudioClip onUnhover = null;

		public UnityEvent OnClick = null;

		//Members
		Animator anim = null;
		AudioSource audSrc = null;
		InfoPanel infoPanel = null;

		void Awake()
		{
			anim = GetComponent<Animator>();
			audSrc = GetComponent<AudioSource>();

			infoPanel = FindObjectOfType<InfoPanel>();

			Debug.Assert(infoPanel, string.Format("[{0}] {1} not found!", this.name, typeof(InfoPanel).Name));
		}

		//Event system callbacks
		public void OnPointerEnter(PointerEventData eventData)
		{
			anim.SetBool("OnHover", true);
			audSrc.PlayOneShot(onHover);
			infoPanel?.ShowPanel(title, 0);
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			anim.SetBool("OnHover", false);
			audSrc.PlayOneShot(onUnhover);
			infoPanel?.UnShowPanel();
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			anim.SetTrigger("OnClick");
			audSrc.PlayOneShot(onClick);
			OnClick.Invoke();
		}
	}
}