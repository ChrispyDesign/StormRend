using StormRend.Systems.StateMachines;
using UnityEngine;
using UnityEngine.Playables;

namespace StormRend.MainMenus 
{ 
	public class Quitter : MonoBehaviour
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