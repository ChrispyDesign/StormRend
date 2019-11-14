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
		[SerializeField] AudioClip gameWinClip;

		UltraStateMachine usm;
		UnitRegistry unitRegistry;
		GameDirector gameDirector;
		GameOverState gameOverPanel;
		VictoryState victoryPanel;
		AudioSource src;

		private void Awake()
		{
			unitRegistry = UnitRegistry.current;
			gameDirector = GameDirector.current;

			gameOverPanel = FindObjectOfType<GameOverState>();
			victoryPanel = FindObjectOfType<VictoryState>();
			src = gameDirector.generalAudioSource;
		}

		public void HaveLost()
		{
			if (unitRegistry.allAlliesDead)
			{
				src.loop = false;

				if (gameLostClip)
				{
					src.clip = gameLostClip;
					src.Play();
				}

				usm.Stack(gameOverPanel);
			}
		}

		public void HaveWon()
		{
			if(unitRegistry.allEnemiesDead)
			{
				src.loop = false;

				if (gameWinClip)
				{
					src.clip = gameWinClip;
					src.Play();
				}

				usm.Stack(victoryPanel);
			}
		}
	}
}