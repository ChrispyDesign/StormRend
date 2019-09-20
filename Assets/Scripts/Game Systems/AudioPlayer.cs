using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StormRend.Audio
{
    [RequireComponent(typeof(AudioSource))]
	public class AudioPlayer : MonoBehaviour
	{
        public AudioClip[] sounds;
		AudioSource audioSource;

		void Awake()
		{
			audioSource = GetComponent<AudioSource>();
		}

		public void PlayRandom()
		{
			audioSource.PlayOneShot(sounds[Random.Range(0, sounds.Length-1)]);
		}
	}
}
