/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

namespace StormRend.Sequencing
{
	[RequireComponent(typeof(PlayableDirector))]
	public class SequenceController : MonoBehaviour
	{
		//Inspector
		[SerializeField] bool onlyPlayOnce = false;

		[Header("Triggerings")]
		[SerializeField] KeyCode triggerKey = KeyCode.E;

		[Header("Events")]
		public UnityEvent OnPlay;
		public UnityEvent OnPause, OnStop;

		//Properties
		public bool isPlaying => playableDirector.state == PlayState.Playing;

		//Privates
		PlayableDirector playableDirector;
		bool firstPlayed = false;

		void Awake()
		{
			playableDirector = GetComponent<PlayableDirector>();
		}

		void Update()
		{
			if (Input.GetKeyDown(triggerKey))
				Play();
		}

		/// <summary>
		/// Start the cutscene sequence
		/// </summary>
		public void Play()
		{
			if (onlyPlayOnce && firstPlayed) 
				return;
			
			playableDirector.Play();
			OnPlay.Invoke();
			firstPlayed = true;
		}

		public void Pause()
		{
			playableDirector.Pause();
			OnPause.Invoke();
		}

		//Might be used to skip cutscenes?
		public void Stop()
		{
			playableDirector.Stop();
			OnStop.Invoke();
		}

	}
}