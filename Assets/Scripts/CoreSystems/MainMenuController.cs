using StormRend.Systems.StateMachines;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StormRend.MainMenus
{
    [RequireComponent(typeof(UltraStateMachine))]
	public class MainMenuController : MonoBehaviour
	{
		//Inspector
		[SerializeField] State introState = null;
		[SerializeField] State tonysIntroState = null;

		//Members
		UltraStateMachine usm;
        void Awake() => usm = GetComponent<UltraStateMachine>();

        void Update()
		{
			//Konami :D
			if (Input.GetKeyDown(KeyCode.T))
				usm.Stack(tonysIntroState);
		}

        public void Play() => usm.Stack(introState);

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
