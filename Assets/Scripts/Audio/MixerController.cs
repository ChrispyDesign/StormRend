using UnityEngine;
using UnityEngine.Audio;

namespace StormRend.Audio 
{ 
	public class MixerController : MonoBehaviour
	{
		[SerializeField] AudioMixer mixer = null;

		[Header("Param Names")]
		[SerializeField] string master = "MasterVol";
		[SerializeField] string music = "MusicVol";
		[SerializeField] string SFX = "SFXVol";
		[SerializeField] string vocals = "VocalsVol";

		void Awake()
		{
			if (!mixer)
			{
				Debug.Log("No mixer found! Disabling...");
				enabled = false;
			}
		}

		public void SetMasterVolume(float value) => mixer.SetFloat(master, value);
		public void SetMusicVolume(float value) => mixer.SetFloat(music, value);
		public void SetSFXVolume(float value) => mixer.SetFloat(SFX, value);
		public void SetVocalsVolume(float value) => mixer.SetFloat(vocals, value);
	}
}