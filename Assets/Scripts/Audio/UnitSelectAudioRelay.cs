using StormRend.Tags;
using StormRend.Units;
using UnityEngine;

namespace StormRend.Audio
{
	public class UnitSelectAudioRelay : AudioRelay
	{

		[Header("On Select Vocals")]
		[SerializeField] AudioMagazine berserkerVocals = null;
		[SerializeField] AudioMagazine valkyrieVocals = null;
		[SerializeField] AudioMagazine sageVocals = null;

		// [Range(0f, 1f), SerializeField] float selectSFXVolume = 0.5f;
		// [SerializeField] AudioClip berserkerSelectSFX = null;
		// [SerializeField] AudioClip valkyrieSelectSFX = null;
		// [SerializeField] AudioClip sageSelectSFX = null;

		void Start()
		{
			Debug.Assert(berserkerVocals, "No Audio Magazine Loaded!");
			Debug.Assert(valkyrieVocals, "No Audio Magazine Loaded!");
			Debug.Assert(sageVocals, "No Audio Magazine Loaded!");
		}

		/// <summary>
		/// Play certain audio magazine according to unit selected
		/// </summary>
		/// <param name="u"></param>
		public void OnUnitSelected(Unit u)
		{
			var au = u as AnimateUnit;

			//Only play vocals if the unit can atleast act
			if (!au.canAct) return;

			switch (u.tag)
			{
				case BerserkerTag b:
					// audioSource.PlayOneShot(berserkerSelectSFX, selectSFXVolume);
					audioSystem.ChancePlayMagazine(berserkerVocals);
					break;
				case ValkyrieTag v:
					// audioSource.PlayOneShot(valkyrieSelectSFX, selectSFXVolume);
					audioSystem.ChancePlayMagazine(valkyrieVocals);
					break;
				case SageTag s:
					// audioSource.PlayOneShot(sageSelectSFX, selectSFXVolume);
					audioSystem.ChancePlayMagazine(sageVocals);
					break;
			}
		}
   	}
}
