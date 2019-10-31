using System.Collections.Generic;
using StormRend.Utility.Attributes;
using UnityEngine;

namespace StormRend.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioSystem : MonoBehaviour
    {
        //Helpbox
        [HelpBox, SerializeField] string help = "Animation Event Callbacks: \nPlayOnce(AudioClip)";

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
            if (Random.Range(0, 100) < chance)
                audioSource.PlayOneShot(sounds[Random.Range(0, sounds.Count)]);
        }
        public void ChancePlay(AudioClip clip)
        {
            if (Random.Range(0, 100) < chance)
                audioSource.PlayOneShot(clip);
        }
        public void ChancePlayAudioContainer(Object o)
        {
            AudioContainer ac = (AudioContainer)o;
            if (Random.Range(0, 100) < ac.chance)
                audioSource.PlayOneShot(ac.clips[Random.Range(0, ac.clips.Count)]);
        }

        /// <summary>
        /// For animation events
        /// </summary>
        /// <param name="o">The sound that is passed in and will be casted to an audio clip</param>
        public void PlayOnce(Object o)
        {
            AudioClip c = o as AudioClip;
            audioSource.PlayOneShot(c);
        }
    #endregion
    }
}