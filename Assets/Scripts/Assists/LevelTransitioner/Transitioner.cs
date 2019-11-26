using StormRend.Systems;
using StormRend.Units;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StormRend
{
	public class Transitioner : MonoBehaviour
	{
		//Maybe just create an audiosource, set it to play on awake and preload the start clip
		[SerializeField] AudioClip startClip;

		GameDirector gameDirector;

		private void Start()
		{
			if (startClip)
			{
				gameDirector = GameDirector.current;
				AudioSource src = gameDirector.SFXAudioSource;
				src.loop = false;

				src.clip = startClip;
				src.Play();
			}	
		}
	}
}