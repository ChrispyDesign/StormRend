/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

using StormRend.Systems;
using StormRend.Systems.StateMachines;
using UnityEngine;
using UnityEngine.Playables;

namespace StormRend.States
{
	/// <summary>
	/// A state for playing narration and vocals etc
	/// </summary>
	[RequireComponent(typeof(PlayableDirector))]
	public class NarrativeState : CoverState
	{
		//Inspector
		[Header("Narration")]
		[SerializeField] protected KeyCode skipKey = KeyCode.Space;
		[SerializeField] protected AudioClip skipSFX = null;

		//Members
		PlayableDirector pd = null;

		void OnEnable()
		{
			pd = GetComponent<PlayableDirector>();

			pd.stopped += SkipRelay;
		}
		void OnDisable()
		{
			pd.stopped -= SkipRelay;
		}

		public override void OnEnter(UltraStateMachine sm)
		{
			base.OnEnter(sm);	//Runs activate/deactivate logic

			pd.Play();
		}

		public override void OnUpdate(UltraStateMachine sm)
		{
			base.OnUpdate(sm);	//Runs activate/deactivate logic

			//Check for manual exit
			if (Input.GetKeyDown(skipKey)) Skip();
		}

		//Callback
		void SkipRelay(PlayableDirector pd) => Skip();
		public void Skip()
		{
			GameDirector.current.sfxAudioSource.PlayOneShot(skipSFX);
			GameDirector.current.SafeSkip();
		}
	}
} 