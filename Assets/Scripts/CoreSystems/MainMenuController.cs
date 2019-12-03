using pokoro.BhaVE.Core.Variables;
using StormRend.Systems.StateMachines;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StormRend.MainMenus
{
	[RequireComponent(typeof(UltraStateMachine))]
	public class MainMenuController : MonoBehaviour
	{
		//Inspector
		[SerializeField] BhaveBool isPlayTonysCutscene = null;
		[SerializeField] State introState = null;
		[SerializeField] State tonysIntroState = null;

		//Members
		UltraStateMachine usm;
		void Awake()
		{
			usm = GetComponent<UltraStateMachine>();
			Debug.Assert(isPlayTonysCutscene != null, "No SOV found!");
		}

		void Start() => isPlayTonysCutscene.value = false;  //Default to play normal cutscene

		public void Play()
		{
			if (isPlayTonysCutscene.value == true)
				usm.Stack(tonysIntroState);
			else
				usm.Stack(introState);
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
	}
}
