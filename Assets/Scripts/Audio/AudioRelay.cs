using UnityEngine;

namespace StormRend.Audio
{

	[RequireComponent(typeof(AudioSystem))]
    public abstract class AudioRelay : MonoBehaviour
    {
		[Header("Will auto find if these components aren't specified")]
		[SerializeField] protected AudioSystem audioSystem = null;
		[SerializeField] protected AudioSource audioSource = null;

		void Awake()
		{
			if (!audioSystem) audioSystem = GetComponent<AudioSystem>();
			if (!audioSource) audioSource = GetComponent<AudioSource>();
		}
    }
}