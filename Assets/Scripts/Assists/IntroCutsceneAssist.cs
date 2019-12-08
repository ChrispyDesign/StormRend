/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

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