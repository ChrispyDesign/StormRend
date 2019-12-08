/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

using UnityEngine;
using UnityEngine.SceneManagement;

namespace StormRend.UI.Utils 
{ 
	public class SceneController : MonoBehaviour
	{
		public void LoadNextScene()
		{
			var currentScene = SceneManager.GetActiveScene().buildIndex;
			SceneManager.LoadScene(currentScene + 1);
		}

		public void LoadScene(string sceneName) => SceneManager.LoadScene(sceneName);
		public void LoadScene(int buildIndex) => SceneManager.LoadScene(buildIndex);
   	}
}