using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StormRend.Audio
{
    public class MuteMusic : MonoBehaviour
    {
        public KeyCode mute = KeyCode.M;
        public AudioSource music;
        void Start()
        {
            music = gameObject.GetComponent(typeof(AudioSource)) as AudioSource;
        }
        void ToggleMusic()
        {
            // toggle this off/on
            if (Input.GetKeyUp(mute))
                // if (gameObject.active == true)
                //     gameObject.SetActive(false);
                // else
                //     gameObject.SetActive(true);
                // gameObject.SetActive(!gameObject.activeInHierarchy);
                music.enabled = !music.enabled;
        }
        void Update()
        {
            ToggleMusic();
        }
    }
}
