using StormRend.Systems;
using StormRend.Units;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StormRend
{
	public class GameEndCondition : MonoBehaviour
	{
		[SerializeField] AudioClip gameLostClip;

		UnitRegistry unitRegistry;
		GameDirector gameDirector;

		private void Awake()
		{
			unitRegistry = UnitRegistry.current;
			gameDirector = GameDirector.current;
		}

		public void HaveLost()
		{
			if (unitRegistry.allAlliesDead)
			{
				AudioSource src = gameDirector.generalAudioSource;
				src.loop = false;
				src.clip = gameLostClip;
				src.Play();
			}
		}
	}
}