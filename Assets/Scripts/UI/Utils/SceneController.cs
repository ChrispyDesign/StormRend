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