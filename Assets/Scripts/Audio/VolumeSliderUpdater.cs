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