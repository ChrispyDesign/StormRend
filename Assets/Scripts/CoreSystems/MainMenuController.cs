/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

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
