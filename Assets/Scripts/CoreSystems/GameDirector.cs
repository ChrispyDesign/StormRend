﻿using pokoro.Patterns.Generic;
using StormRend.Assists;
using StormRend.Audio;
using StormRend.States;
using StormRend.Systems.StateMachines;
using StormRend.Units;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StormRend.Systems
{
	//Brainstorm:
	//- All buttons or things that wants to pause the game must go through here
	//- prevent from pause if in victory, defeat or end turn confirmation states

	/// <summary>
	/// Class that helps controls the game state machine as well as other side functionality such as:
	/// Handling pause, has functions to change scenes, go back to main menu, handling end game, etc
	/// </summary>
	[RequireComponent(typeof(UltraStateMachine), typeof(AudioSystem))]
	public class GameDirector : Singleton<GameDirector>
	{
		//Inspector
		[Header("Game Pause")]
		[SerializeField] KeyCode pauseKey = KeyCode.Escape;
		[SerializeField] PauseMenuState pauseMenuState = null;

		[Header("Turn States")]
		[SerializeField] EndTurnConfirmState endTurnConfirmationState = null;

		[Header("End States")]
		[SerializeField] AudioClip victoryNarration = null;
		[SerializeField] State victoryState = null;
		[SerializeField] AudioClip defeatNarration = null;
		[SerializeField] State defeatState = null;

		[Header("Scene Management")]
		public string mainMenuSceneName = null;
		public string nextSceneName = null;

		//Properties
		public State currentState => usm?.currentState;
		public AudioSource generalAudioSource => audioSource;
		public AudioSystem generalAudioSystem => audioSystem;

		//Members
		AudioSystem audioSystem = null;
		AudioSource audioSource = null;
		UltraStateMachine usm = null;
		UnitRegistry ur = null;
		AllActionsUsedChecker actionsUsedChecker = null;

		//Debugs
		[SerializeField, Space(10)] bool debug = false;

		#region Init
		void Awake()
		{
			//Audio
			audioSystem = GetComponent<AudioSystem>();
			audioSource = GetComponent<AudioSource>();

			//Systems
			usm = GetComponent<UltraStateMachine>();
			ur = UnitRegistry.current;
			actionsUsedChecker = FindObjectOfType<AllActionsUsedChecker>();
		}

		void Start()
		{
			Debug.Assert(pauseMenuState, "No Pause Menu State Found!");
			Debug.Assert(endTurnConfirmationState, "No End Turn Confirmation State Found!");
			Debug.Assert(actionsUsedChecker, "No All Actions Used Checker Found!");
		}

		//Register events
		void OnEnable() => ur.onUnitKilled.AddListener(CheckEndGame);
		void OnDisable() => ur.onUnitKilled.RemoveListener(CheckEndGame);
		#endregion

		#region State Management
		void Update() => HandlePause();
		void HandlePause()
		{
			if (Input.GetKeyDown(pauseKey))
			{
				if (currentState == pauseMenuState)             //NOT already paused
					UnpauseGame();
				else
					PauseGame();
			}
		}

		/// <summary>
		/// Call back to check and run end game sequences
		/// </summary>
		public void CheckEndGame(Unit u)
		{
			if (ur.allAlliesDead)
			{
				usm.Stack(defeatState);
				audioSource.PlayOneShot(defeatNarration);
			}
			else if (ur.allEnemiesDead)
			{
				usm.Stack(victoryState);
				audioSource.PlayOneShot(victoryNarration);
			}
		}

		/// <summary>
		/// Safe pause game function that can be called anytime, anywhere by anything
		/// </summary>
		public void PauseGame() => TryPauseGame();

		/// <summary>
		/// Safe and checked pause game function
		/// </summary>
		internal void TryPauseGame()
		{
			//Can only pause if the game if...
			if (currentState != pauseMenuState &&               //NOT already paused
				currentState != endTurnConfirmationState &&		//NOT showing the victory menu
				currentState != victoryState &&                 //NOT showing the defeat menu
				currentState != defeatState)                    //NOT showing the end turn confirm dialog
			{
				usm.Stack(pauseMenuState);
			}
		}
		//There should only be one major pause menu. Unpausing the game means completely unstacking the state machine
		public void UnpauseGame() => usm.ClearStack();

		/// <summary>
		/// Go to the next turn if it is safe to do so
		/// </summary>
		public void SafeNextTurn()
		{
			if (actionsUsedChecker.isAllActionsUsedUp)
			{
				//safe to go to next turn
				actionsUsedChecker.StopTimer();
				usm.ClearStack();
				usm.NextTurn();	
			}
			else
			{
				//Confirm with the user
				usm.Stack(endTurnConfirmationState);
			}
		}

		//To be referenced by EndTurnConfirmation.NextTurnButton
		public void ForcedNextTurn()
		{
			usm.ClearStack();
			usm.NextTurn();
		}
		#endregion

		#region Scene Management
		public void ReloadCurrentScene()
		{
			Time.timeScale = 1f;
			var currentScene = SceneManager.GetActiveScene();
			SceneManager.LoadScene(currentScene.buildIndex);
		}
		public void LoadMainMenu()
		{
			//Reset time scale from any pausing
			Time.timeScale = 1f;
			//Load
			SceneManager.LoadScene(mainMenuSceneName);
		}
		public void LoadNextScene()
		{
			Time.timeScale = 1f;
			SceneManager.LoadScene(nextSceneName);
		}
		public void LoadScene(string scene)
		{
			//Reset timescale
			Time.timeScale = 1f;
			SceneManager.LoadScene(scene);
		}
		public void LoadSceneIDX(int buildIDX)
		{
			Time.timeScale = 1f;
			SceneManager.LoadScene(buildIDX);
		}
		public void QuitGame()
		{
			Debug.Log("Quitting...");
			Application.Quit();
		}
		#endregion

		#region Debug
		void OnGUI()
		{
			if (!debug) return;
			if (GUILayout.Button("PrevTurn")) usm.PrevTurn();
			if (GUILayout.Button("Kill Allies")) KillAllUnitsOfType<AllyUnit>();
			if (GUILayout.Button("Kill Enemies")) KillAllUnitsOfType<EnemyUnit>();
			if (GUILayout.Button("Destroy All InAnimates")) KillAllUnitsOfType<InAnimateUnit>();

			void KillAllUnitsOfType<T>() where T : Unit
			{
				foreach (var u in ur.GetAliveUnitsByType<T>())
				{
					var au = u as AnimateUnit;
					var iau = u as InAnimateUnit;
					if (au)
						au.Kill();
					else if (iau)
						iau.TakeDamage(new HealthData(null, 9999999));
					else
						u.TakeDamage(new HealthData(null, 9999999));
				}
			}
		}
		#endregion
	}
}