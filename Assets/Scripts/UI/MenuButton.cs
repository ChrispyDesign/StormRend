using UnityEngine;
using UnityEngine.EventSystems;

namespace StormRend.UI 
{
	/// <summary>
	/// This just finds the first audio source
	/// </summary>
	public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
	{
		//Inspector
		[SerializeField] AudioClip onClick = null;
		[SerializeField] AudioClip onHover = null;
		[SerializeField] AudioClip onUnhover = null;		

		//Members
		AudioSource audioSource = null;

		void Awake()
		{
			audioSource = FindObjectOfType<AudioSource>();

			//Fail safe
			if (!audioSource)
			{
				Debug.LogWarning("No existing audiosource found. Creating...");
				GameObject go = new GameObject("AudioSource (Runtime Created)", typeof(AudioSource));
				go.transform.SetParent(null);
				audioSource = go.GetComponent<AudioSource>();
			}
		}

		public void OnPointerClick(PointerEventData eventData) => audioSource.PlayOneShot(onClick);
		public void OnPointerEnter(PointerEventData eventData) => audioSource.PlayOneShot(onHover);
		public void OnPointerExit(PointerEventData eventData) => audioSource.PlayOneShot(onUnhover);
	}
}