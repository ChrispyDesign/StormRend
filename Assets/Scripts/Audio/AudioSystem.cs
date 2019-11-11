using System.Collections.Generic;
using StormRend.Utility.Attributes;
using UnityEngine;

namespace StormRend.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioSystem : MonoBehaviour
    {
        //Inspector
        [TextArea(0, 2), SerializeField] string description = " ";

        [Tooltip("Chance of playing a sound when triggered as a percentage")]
        [Range(0, 100), SerializeField] int chance = 50;

        [SerializeField] List<AudioClip> sounds = new List<AudioClip>();

        //Members
        AudioSource audioSource;

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
    #region Play by Chance
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
    #endregion

    #region Animation Event Handlers
        /// <summary> Play one audio clip out of a magazine according to chance </summary>
        /// <param name="audioMagazine">AudioMagazine scriptable object</param>
        public void ChancePlayMagazine(Object audioMagazine)
        {
            AudioMagazine am = audioMagazine as AudioMagazine;
            if (Random.Range(0, 100) < am.chance)
                audioSource.PlayOneShot(am.clips[Random.Range(0, am.clips.Count)], am.volume);
        }

        /// <summary> Play one audio clip once </summary>
        /// <param name="audioClip">Single AudioClip</param>
        public void PlayOnce(Object audioClip)
        {
            AudioClip c = audioClip as AudioClip;
            audioSource.PlayOneShot(c);
        }
    #endregion
    }
}