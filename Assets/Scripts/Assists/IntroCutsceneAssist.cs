using StormRend.UI.Utils;
using UnityEngine;
using UnityEngine.Playables;

namespace StormRend.Assists 
{ 
	/// <summary>
	/// Automatically goes to the next scene when the intro cutscene has finished playing
	/// </summary>
	public class IntroCutsceneAssist : MonoBehaviour
	{	
		PlayableDirector playableDirector;
		SceneController sc;

		void Awake()
		{
			playableDirector = GetComponent<PlayableDirector>();
			sc = GetComponent<SceneController>();
		}

		void OnEnable() => playableDirector.stopped += OnCutSceneFinished;
		void OnDisable() => playableDirector.stopped -= OnCutSceneFinished;

		void OnCutSceneFinished(PlayableDirector playableDirector)
		{
			sc.LoadNextScene();
		}
   	}
}