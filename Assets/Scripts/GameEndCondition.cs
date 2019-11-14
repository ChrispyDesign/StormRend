using StormRend.Systems;
using StormRend.Systems.StateMachines;
using StormRend.UI;
using StormRend.Units;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StormRend
{
	public class GameEndCondition : MonoBehaviour
	{
		[SerializeField] AudioClip gameLostClip;

		UltraStateMachine usm;
		UnitRegistry unitRegistry;
		GameDirector gameDirector;
		GameOverState gameOverPanel;
		VictoryState victoryPanel;

		private void Awake()
		{
			unitRegistry = UnitRegistry.current;
			gameDirector = GameDirector.current;

			gameOverPanel = FindObjectOfType<GameOverState>();
			victoryPanel = FindObjectOfType<VictoryState>();
		}

		public void HaveLost()
		{
			if (unitRegistry.allAlliesDead)
			{
				AudioSource src = gameDirector.generalAudioSource;
				src.loop = false;
				src.clip = gameLostClip;
				src.Play();

				usm.Stack(gameOverPanel);
			}
		}
	}
}