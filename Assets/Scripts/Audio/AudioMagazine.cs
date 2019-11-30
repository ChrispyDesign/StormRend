using System.Collections.Generic;
using UnityEngine;

namespace StormRend.Audio
{
    [CreateAssetMenu(menuName = "StormRend/AudioMagazine")]
    public class AudioMagazine : ScriptableObject
    {
		[Range(0f, 1f)] public float volume = 1f;
        [Range(0, 100)] public int chance = 50;

		//Members
		internal AudioClip lastPlayed = null;

        public List<AudioClip> clips;
    }
}