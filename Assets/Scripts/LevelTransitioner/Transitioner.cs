using StormRend.Systems;
using StormRend.Units;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StormRend
{
	public class Transitioner : MonoBehaviour
	{
		[SerializeField] AudioClip startClip;

		GameDirector gameDirector;

		private void Start()
		{
			gameDirector = GameDirector.current;
			AudioSource src = gameDirector.generalAudioSource;
			src.loop = false;

			src.clip = startClip;
			src.Play();			
		}
	}
}