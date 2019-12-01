using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StormRend.Audio
{
    public class MuteMusic : MonoBehaviour
    {
        [SerializeField] KeyCode muteKey = KeyCode.M;
        [SerializeField] AudioSource audioSource;

		void Awake() => audioSource = GetComponent<AudioSource>();
		void Update()
        {
			if (Input.GetKeyUp(muteKey))
            	ToggleMusic();
        }
		void ToggleMusic() => audioSource.mute = !audioSource.mute;
    }
}
