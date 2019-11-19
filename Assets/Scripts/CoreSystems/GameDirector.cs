using pokoro.Patterns.Generic;
using StormRend.Audio;
using StormRend.States;
using StormRend.Systems.StateMachines;
using StormRend.Units;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StormRend.Systems
{
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

		[Header("End States")]
		[SerializeField] AudioClip victoryNarration = null;
		[SerializeField] State victoryState = null;
		[SerializeField] AudioClip defeatNarration = null;
		[SerializeField] State defeatState = null;

		[Header("Scene Management")]
		public string mainMenuSceneName = null;
		public string nextSceneName = null;

		//Properties
		public State currentGameState => usm?.currentState;
		public AudioSource generalAudioSource => audioSource;
		public AudioSystem generalAudioSystem => audioSystem;

		//Members
		AudioSystem audioSystem = null;
		AudioSource audioSource = null;
		UltraStateMachine usm = null;
		UnitRegistry ur = null;

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
		}

		void Start()
		{
			if (!pauseMenuState)
			{
				Debug.LogWarning("No Pause Menu State Found! Disabling Game Director...");
				enabled = false;
			}
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
				if (usm.currentState != pauseMenuState)
					PauseGame();
				else
					UnpauseGame();
			}
		}

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

		public void PauseGame() => usm.Stack(pauseMenuState);
		public void UnpauseGame() => usm.UnStack();
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
		#endregion

		#region Debug
		void OnGUI()
		{
			if (!debug) return;
			if (GUILayout.Button("PrevTurn")) usm.PrevTurn();
			if (GUILayout.Button("Kill Allies")) KillAllUnitsOfType<AllyUnit>();
			if (GUILayout.Button("Kill Enemies")) KillAllUnitsOfType<EnemyUnit>();

			void KillAllUnitsOfType<T>() where T : Unit
			{
				foreach (var u in ur.GetAliveUnitsByType<T>())
				{
					var au = u as AnimateUnit;
					if (au)
						au.TakeDamage(new HealthData(null, 1000));
					else
						u.TakeDamage(new HealthData(null, 1000));
				}
			}
		}
		#endregion
	}
}