using StormRend.Audio;
using StormRend.Tags;
using StormRend.Units;
using UnityEngine;

namespace StormRend.Audio
{
	public class UnitSelectAudioPlayer : AudioPlayer
	{
		[SerializeField] AudioMagazine berserkerOnSelectSFX;
		[SerializeField] AudioMagazine valkyrieOnSelectSFX;
		[SerializeField] AudioMagazine sageOnSelectSFX;

		void Start()
		{
			Debug.Assert(berserkerOnSelectSFX, "No Audio Magazine Loaded!");
			Debug.Assert(valkyrieOnSelectSFX, "No Audio Magazine Loaded!");
			Debug.Assert(sageOnSelectSFX, "No Audio Magazine Loaded!");
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
					audioSystem.ChancePlayMagazine(berserkerOnSelectSFX);
					break;
				case ValkyrieTag v:
					audioSystem.ChancePlayMagazine(valkyrieOnSelectSFX);
					break;
				case SageTag s:
					audioSystem.ChancePlayMagazine(sageOnSelectSFX);
					break;
			}
		}
   	}
}
