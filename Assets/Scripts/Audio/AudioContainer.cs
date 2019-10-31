using System.Collections.Generic;
using UnityEngine;

namespace StormRend.Audio
{
    [CreateAssetMenu(menuName = "StormRend/AudioContainer")]
    public class AudioContainer : ScriptableObject
    {
        [Range(0, 100)] public int chance = 50;
        public List<AudioClip> clips;
    }
}