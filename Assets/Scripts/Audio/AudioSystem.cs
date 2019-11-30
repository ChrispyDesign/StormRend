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
			audioSource.mute = false;
			audioSource.loop = false;
		}

		#region Play by Chance
		/// <summary>
		/// Plays the current loaded magazine according to the chance setting on this audio system
		/// </summary>
		public void ChancePlay()
		{
			if (!magazine) return;

			//Don't play the same sound over and over again
			while (true)
			{
				AudioClip clipToPlay = magazine.clips[Random.Range(0, magazine.clips.Count)];
				if (clipToPlay == magazine.lastPlayed) continue;

				//Play by chance
				if (Random.Range(0, 100) < magazine.chance)
				{
					audioSource.PlayOneShot(clipToPlay, magazine.volume);
					magazine.lastPlayed = clipToPlay;
				}
				break;
			}
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
			//Cast
			AudioMagazine am = audioMagazine as AudioMagazine;
			if (!am)
			{
				print("Not an audio magazine!");
				return;
			}

			//Don't play the same sound over and over again
			//NOTE: With brief testing, the max iterations was 3. Usually 1 or 2. Performance is fine
			while (true)
			{
				AudioClip clipToPlay = am.clips[Random.Range(0, am.clips.Count)];
				if (clipToPlay == am.lastPlayed) continue;

				//Play by chance
				if (Random.Range(0, 100) < am.chance)
				{
					audioSource.PlayOneShot(clipToPlay, am.volume);
					am.lastPlayed = clipToPlay;
				}
				break;
			}
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