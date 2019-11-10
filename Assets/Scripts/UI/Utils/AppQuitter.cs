using UnityEngine;

namespace StormRend.MainMenus
{
	public class AppQuitter : MonoBehaviour
	{
		/// <summary>
		/// Quit the application [CALLBACK]
		/// </summary>
		public void Quit()
		{
			Debug.Log("Quitting");
			Application.Quit();
		}
   	}
}