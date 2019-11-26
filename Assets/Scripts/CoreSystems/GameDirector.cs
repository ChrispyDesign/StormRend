using System.Collections;
using System.Linq;
using pokoro.Patterns.Generic;
using StormRend.Abilities;
using StormRend.Assists;
using StormRend.Audio;
using StormRend.States;
using StormRend.Systems.StateMachines;
using StormRend.Tags;
using StormRend.Units;
using StormRend.Utility.Events;
using UnityEngine;
using UnityEngine.Events;
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

		[Header("Events")]
		public UnityEvent onUnitActed = null;
		public UnitEvent onUnitKilled = null;

		//Properties
		public State currentState => usm?.currentState;
		public AudioSource generalAudioSource => audioSource;
		public AudioSystem generalAudioSystem => audioSystem;

		//Members
		AudioSystem audioSystem = null;
		AudioSource audioSource = null;
		UltraStateMachine usm = null;
		UnitRegistry ur = null;
		UserInputHandler input = null;
		AllActionsUsedChecker actionsUsedChecker = null;
		bool gameEnded = false;

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
			input = FindObjectOfType<UserInputHandler>();
			actionsUsedChecker = FindObjectOfType<AllActionsUsedChecker>();
		}

		void Start()
		{
			Debug.Assert(pauseMenuState, "No Pause Menu State Found!");
			Debug.Assert(endTurnConfirmationState, "No End Turn Confirmation State Found!");
			Debug.Assert(input, "No User Input Handler found!");
			Debug.Assert(actionsUsedChecker, "No All Actions Used Checker Found!");

			//Register events for check game ending
			var animateUnits = ur.GetAliveUnitsByType<AnimateUnit>();
			if (animateUnits.Length > 0)    //Does this need a null check?
				foreach (var au in animateUnits)
					au.onTakeDamage.AddListener(OnUnitTakeDamage);
		}

		//Register events
		void OnEnable() => ur.onUnitKilled.AddListener(OnUnitKilled);
		void OnDisable() => ur.onUnitKilled.RemoveListener(OnUnitKilled);
		#endregion

		#region Callbacks
		void OnUnitTakeDamage(HealthData data)
		{
			onUnitActed?.Invoke();

			CheckAndPerformGameEnding();
		}

		void OnUnitKilled(Unit u)
		{
			onUnitKilled?.Invoke(u);

			CheckAndPerformGameEnd();
		}

		/// <summary>
		/// Call back to check and run end game sequences
		/// </summary>
		public void CheckAndPerformGameEnd()
		{
			if (gameEnded) return;

			if (ur.allAlliesDead)
			{
				gameEnded = true;
				usm.Stack(defeatState);
				audioSource.PlayOneShot(defeatNarration);
			}
			else if (ur.allEnemiesDead)
			{
				gameEnded = true;
				usm.Stack(victoryState);
				audioSource.PlayOneShot(victoryNarration);
			}
		}

		/// <summary>
		/// Checks if game is ending and prepares for it
		/// </summary>
		public void CheckAndPerformGameEnding()
		{
			// Debug.Log("Check and perform game ending");
			if (ur.allAlliesDead || ur.allEnemiesDead)
			{
				//Game is ending so stop user from doing anymore input
				input.enabled = false;
				actionsUsedChecker.Stop();
				actionsUsedChecker.enabled = false;
			}
		}

		IEnumerator DelayedCheck(float delay)
		{
			yield return new WaitForSeconds(delay);
			if (ur.allAlliesDead || ur.allEnemiesDead)
			{
				//Game is ending so stop user from doing anymore input
				input.enabled = false;
				actionsUsedChecker.Stop();
			}
		}
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
				currentState != endTurnConfirmationState &&     //NOT showing the victory menu
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
				actionsUsedChecker.Stop();
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
			if (GUILayout.Button("Kill Berserker")) KillUnitByType<BerserkerTag>();
			if (GUILayout.Button("Kill Valkyrie")) KillUnitByType<ValkyrieTag>();
			if (GUILayout.Button("Kill Sage")) KillUnitByType<SageTag>();

			if (GUILayout.Button("Kill Enemies")) KillAllUnitsOfType<EnemyUnit>();
			if (GUILayout.Button("Kill Frost Troll")) KillUnitByType<FrostTrollTag>();
			if (GUILayout.Button("Kill Frost Hound")) KillUnitByType<FrostHoundTag>();

			if (GUILayout.Button("Destroy All InAnimates")) KillAllUnitsOfType<InAnimateUnit>();


			void KillUnitByType<T>() where T : UnitTag
			{
				var unitToKill = (from unit in ur.aliveUnits where unit.tag is T select unit).First();
				if (unitToKill) KillUnit(unitToKill);
			}

			void KillAllUnitsOfType<T>() where T : Unit
			{
				foreach (var u in ur.GetAliveUnitsByType<T>())
					KillUnit(u);
			}

			void KillUnit(Unit u)
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
		#endregion
	}
}