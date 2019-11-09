using StormRend.Audio;
using StormRend.Tags;
using StormRend.Units;
using UnityEngine;

namespace StormRend.Audio
{
	public class UnitSelectAudioPlayer : AudioPlayer
	{
		[Range(0f, 1f), SerializeField] float selectVolume = 0.5f;

		[Header("Berserker")]
		[SerializeField] AudioMagazine berserkerVocals;
		[SerializeField] AudioClip berserkerSelect;

		[Header("Valkyrie")]
		[SerializeField] AudioMagazine valkyrieVocals;
		[SerializeField] AudioClip valkyrieSelect;

		[Header("Sage")]
		[SerializeField] AudioMagazine sageVocals;
		[SerializeField] AudioClip sageSelect;

		void Start()
		{
			Debug.Assert(berserkerVocals, "No Audio Magazine Loaded!");
			Debug.Assert(valkyrieVocals, "No Audio Magazine Loaded!");
			Debug.Assert(sageVocals, "No Audio Magazine Loaded!");

			// Debug.Assert(berserkerSelect, "No Audio Clip Loaded!");
			// Debug.Assert(valkyrieSelect, "No Audio Clip Loaded!");
			// Debug.Assert(sageSelect, "No Audio Clip Loaded!");
		}

		/// <summary>
		/// Play certain audio magazine of 
		/// </summary>
		/// <param name="u"></param>
		public void OnUnitChanged(Unit u)
		{
			switch (u.tag)
			{
				case BerserkerTag b:
					audioSource.PlayOneShot(berserkerSelect, selectVolume);
					audioSystem.ChancePlayMagazine(berserkerVocals);
					break;
				case ValkyrieTag v:
					audioSource.PlayOneShot(valkyrieSelect, selectVolume);
					audioSystem.ChancePlayMagazine(valkyrieVocals);
					break;
				case SageTag s:
					audioSource.PlayOneShot(sageSelect, selectVolume);
					audioSystem.ChancePlayMagazine(sageVocals);
					break;
			}
		}
   	}
}
