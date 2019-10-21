using pokoro.Patterns.Generic;
using StormRend.Defunct;
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
	[RequireComponent(typeof(UltraStateMachine))]
	public class SRGameDirector : Singleton<SRGameDirector>
	{
		//Inspector
		[Header("Game Pause")]
		[SerializeField] KeyCode pauseKey = KeyCode.Escape;
		[SerializeField] UIState pauseMenuState;

		[Header("End Game")]
		[SerializeField] State loseState;
		[SerializeField] State victoryState;

		[Header("Scene Management")]
		public int mainMenuSceneIdx = 0;

		//Properties
		public State currentGameState => usm?.currentState;

		//Members
		UltraStateMachine usm;
		bool isPaused;

		//Debugs
		[SerializeField, Space(10)] bool debug = false;

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
			SceneManager.LoadScene(mainMenuSceneIdx);
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