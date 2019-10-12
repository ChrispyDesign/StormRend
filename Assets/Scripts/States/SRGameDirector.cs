using StormRend.Defunct;
using StormRend.States.UI;
using StormRend.Systems.StateMachines;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StormRend.States
{
	[RequireComponent(typeof(UltraStateMachine))]
	public class SRGameDirector : MonoBehaviour
	{
		[SerializeField] bool debug = false;

		[Header("Game Pause")]
		[SerializeField] KeyCode pauseKey = KeyCode.Escape;
		[SerializeField] UIState pauseMenuState;

		[Header("End Game")]
		[SerializeField] State loseState;
		[SerializeField] State victoryState;

		UltraStateMachine usm;
		private bool isPaused;

		void Awake()    //Doesn't matter if you override or hide. Singleton.Awake() will run regardless.
		{
			usm = GetComponent<UltraStateMachine>();
		}

		void Start()
		{
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

		//-------- Temp ----------
		public void ReloadCurrentScene()
		{
			var currentScene = SceneManager.GetActiveScene();
			SceneManager.LoadScene(currentScene.buildIndex);
		}

		[Header("Scene Management")]
		public int mainMenuSceneIdx = 0;
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