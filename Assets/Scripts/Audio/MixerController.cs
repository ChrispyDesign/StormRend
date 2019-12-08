/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

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