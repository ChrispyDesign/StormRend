/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

using StormRend.States;
using StormRend.Systems;
using StormRend.Systems.StateMachines;
using StormRend.Units;
using UnityEngine;

namespace StormRend
{
	public class GameEndCondition : MonoBehaviour
	{
		[SerializeField] AudioClip gameLostClip;
		[SerializeField] AudioClip gameWinClip;
		[SerializeField] VictoryState victoryPanel;
		[SerializeField] DefeatState gameOverPanel;

		UltraStateMachine usm;
		UnitRegistry unitRegistry;
		GameDirector gameDirector;
		AudioSource src;

		private void Awake()
		{
			unitRegistry = UnitRegistry.current;
			gameDirector = GameDirector.current;
			usm = FindObjectOfType<UltraStateMachine>();

			src = gameDirector.sfxAudioSource;
		}

		private void Update()
		{
			//TEST ONLY
			if (Input.GetKeyDown(KeyCode.U))
			{
				AllyUnit[] allyunits = unitRegistry.GetAliveUnitsByType<AllyUnit>();

				foreach (AllyUnit ally in allyunits)
				{
					ally.TakeDamage(new HealthData(ally, 5));
				}
				HaveLost();
			}
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
			if (unitRegistry.allEnemiesDead)
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