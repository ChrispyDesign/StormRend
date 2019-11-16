using UnityEngine;

namespace StormRend.Audio
{
	[RequireComponent(typeof(AudioSystem))]
    public abstract class AudioRelay : MonoBehaviour
    {
		protected AudioSystem audioSystem;
		protected AudioSource audioSource;

		void Awake()
		{
			audioSystem = GetComponent<AudioSystem>();
			audioSource = GetComponent<AudioSource>();
		}
    }
}