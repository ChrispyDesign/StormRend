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
		[SerializeField] VictoryState victoryPanel;
		[SerializeField] GameOverState gameOverPanel;

		UltraStateMachine usm;
		UnitRegistry unitRegistry;
		GameDirector gameDirector;
		AudioSource src;

		private void Awake()
		{
			unitRegistry = UnitRegistry.current;
			gameDirector = GameDirector.current;
			usm = FindObjectOfType<UltraStateMachine>();

			src = gameDirector.generalAudioSource;
		}

		private void Update()
		{
			if(Input.GetKeyDown(KeyCode.U))
			{
				AllyUnit[] allyunits = unitRegistry.GetAliveUnitsByType<AllyUnit>();

				foreach(AllyUnit ally in allyunits)
				{
					ally.TakeDamage(new HealthData(ally, 5));
				}
			}
			HaveLost();
		}

		public void HaveLost()
		{
			if (unitRegistry.allAlliesDead)
			{
				usm.Stack(gameOverPanel);
				src.loop = false;

				if (gameLostClip)
				{
					src.clip = gameLostClip;
					src.Play();
				}
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