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