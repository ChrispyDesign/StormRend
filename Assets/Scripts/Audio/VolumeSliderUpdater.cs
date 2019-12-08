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
using UnityEngine.UI;

namespace StormRend.UI
{
	/// <summary>
	/// Small class to update the volume sliders upon entering the options menu
	/// </summary>
	[RequireComponent(typeof(Slider))]
	public class VolumeSliderUpdater : UIUpdater
	{
		[SerializeField] AudioMixer mixer = null;
		[SerializeField] string paramName = "MasterVol";

		Slider slider = null;

		void Awake()
		{
			slider = GetComponent<Slider>();
			Debug.Assert(mixer, "No mixer found!");
		}

		void OnEnable()
		{
			if (mixer.GetFloat(paramName, out float value))
				slider.value = value;
		}
	}
}