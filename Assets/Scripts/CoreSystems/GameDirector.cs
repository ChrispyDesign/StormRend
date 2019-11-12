using pokoro.Patterns.Generic;
using StormRend.Audio;
using StormRend.States.UI;
using StormRend.Systems.StateMachines;
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
		[SerializeField] OnState pauseMenuState = null;

		[Header("Scene Management")]
		public string mainMenuSceneName = "MainMenu";

		//Properties
		public State currentGameState => usm?.currentState;
		public AudioSource generalAudioSource => audioSource;
		public AudioSystem generalAudioSystem => audioSystem;

		//Members
		UltraStateMachine usm = null;
		AudioSystem audioSystem = null;
		AudioSource audioSource = null;

		//Debugs
		[SerializeField, Space(10)] bool debug = false;

		void Awake()
		{
			audioSystem = GetComponent<AudioSystem>();
			audioSource = GetComponent<AudioSource>();
		}

		void Start()
		{
			usm = GetComponent<UltraStateMachine>();

			if (!pauseMenuState)
			{
				Debug.LogWarning("No pause menu state found!");
				enabled = false;
			}
		}

		void Update()
		{
			HandlePause();
		}

		void HandlePause()
		{
			if (Input.GetKeyDown(pauseKey))
			{
				// Debug.Log("currentState: " + usm.currentState.GetType().Name);
				if (usm.currentState != pauseMenuState)
				{
					usm.Stack(pauseMenuState);
				}
				else
				{
					usm.ClearStack();
				}
			}
		}

		public void ReturnToGame()
		{
			//Just need to clear the stack in order to return to the turns
			usm.ClearStack();
		}

		public void ReloadCurrentScene()
		{
			var currentScene = SceneManager.GetActiveScene();
			SceneManager.LoadScene(currentScene.buildIndex);
		}


		public void LoadMainMenuScene()
		{
			SceneManager.LoadScene(mainMenuSceneName);
		}

		//----------------------- DEBUG -----------------------
		void OnGUI()
		{
			if (debug)
			{
				if (GUILayout.Button("PrevTurn"))
				{
					usm.PrevTurn();
				}
			}
		}
	}
}