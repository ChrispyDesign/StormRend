using UnityEngine;

namespace StormRend.Audio
{
	[RequireComponent(typeof(AudioSystem))]
    public abstract class AudioPlayer : MonoBehaviour
    {
		protected AudioSystem audioSystem;

		void Awake()
		{
			audioSystem = GetComponent<AudioSystem>();
		}
    }
}