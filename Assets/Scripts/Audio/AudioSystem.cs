using UnityEngine;

namespace StormRend.Audio
{
	[RequireComponent(typeof(AudioSource))]
	public class AudioSystem : MonoBehaviour
	{
		//Inspector
		[TextArea(0, 2), SerializeField] string description = null;

		[Tooltip("Chance of clip playing. OVERRIDES the magazine's chance")]
		[Range(0, 100), SerializeField] int chance = 50;
		[SerializeField] AudioMagazine magazine = null;

		//Members
		AudioSource audioSource;
		void Awake()
		{
			if (!audioSource) audioSource = GetComponent<AudioSource>();
		}
		void OnValidate()
		{
			//No need to check for null because a audio source should always be attached to this gameobject
			audioSource = GetComponent<AudioSource>();
			audioSource.playOnAwake = false;
			audioSource.mute = false;
			audioSource.loop = false;
		}

		#region Play by Chance
		/// <summary>
		/// Plays the current loaded magazine according to the chance setting on this audio system
		/// </summary>
		public void ChancePlay()
		{
			if (magazine && Random.Range(0, 100) < chance)      //null checked
				audioSource.PlayOneShot(magazine.clips[Random.Range(0, magazine.clips.Count)]);
		}
		/// <summary>
		/// Plays the selected clip according to the chance setting of this audio system
		/// </summary>
		/// <param name="clip">The audio clip to play based on chance</param>
		public void ChancePlay(AudioClip clip)
		{
			if (Random.Range(0, 100) < chance)
				audioSource.PlayOneShot(clip);
		}
		#endregion

		#region Animation Event Handlers
		public void ChancePlayClip(Object audioClip)
		{
			AudioClip ac = audioClip as AudioClip;
			if (Random.Range(0, 100) < chance)
				audioSource.PlayOneShot(ac);
		}

		/// <summary> Play one audio clip out of a magazine according to chance </summary>
		/// <param name="audioMagazine">AudioMagazine scriptable object</param>
		public void ChancePlayMagazine(Object audioMagazine)
		{
			AudioMagazine am = audioMagazine as AudioMagazine;
			if (am && Random.Range(0, 100) < am.chance)     //null checked
				audioSource.PlayOneShot(am.clips[Random.Range(0, am.clips.Count)], am.volume);
		}

		/// <summary> Play one audio clip once </summary>
		/// <param name="audioClip">Single AudioClip</param>
		public void PlayOnce(Object audioClip)
		{
			AudioClip ac = audioClip as AudioClip;
			audioSource.PlayOneShot(ac);
		}
		#endregion
	}
}