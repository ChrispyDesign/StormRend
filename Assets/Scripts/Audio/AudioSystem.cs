using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StormRend.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioSystem : MonoBehaviour
    {
        //Inspector
        [TextArea(0, 2), SerializeField] string description = "";

        [Tooltip("Chance of playing a sound when triggered as a percentage")]
        [Range(0, 100), SerializeField] int chance = 50;

        [SerializeField] List<AudioClip> sounds = new List<AudioClip>();

        //Members
        AudioSource audioSource;

    #region Core
        void OnValidate()
        {
            //No need to check for null because a audio source should always be attached to this gameobject
            audioSource = GetComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.mute = false;
            audioSource.loop = false;
        }
        void Awake()
        {
            if (!audioSource) audioSource = GetComponent<AudioSource>();
        }
        public void ChancePlay()
        {
            //if chance is 100 then return true;
            //if chance is 50 then return true half the time;
            //if chance is 0 then always return false;
            if (Random.Range(0, 100) < chance)  
                audioSource.PlayOneShot(sounds[Random.Range(0, sounds.Count)]);
        }
        public void ChancePlay(AudioClip clip)
        {
            if (Random.Range(0, 100) < chance)  
                audioSource.PlayOneShot(clip);
        }
    #endregion
    }
}