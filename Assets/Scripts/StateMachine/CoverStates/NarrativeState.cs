using StormRend.Systems;
using StormRend.Systems.StateMachines;
using UnityEngine;

namespace StormRend.States
{
	/// <summary>
	/// A state for playing narration and vocals etc
	/// </summary>
	[RequireComponent(typeof(AudioSource))]
	public class NarrativeState : CoverState
	{
		//Inspector
		[Header("Narration")]
		[SerializeField] protected AudioClip narrativeClip = null;
		[SerializeField] protected KeyCode skipKey = KeyCode.Space;

		//Members
		GameDirector gd = null;
		AudioSource audioSrc;

		void OnEnable()
		{
			gd = GameDirector.current;
			audioSrc = GetComponent<AudioSource>();
			if (!narrativeClip) Debug.LogWarningFormat("No narrative clip found in {0}", this.name);
		}

		public override void OnEnter(UltraStateMachine sm)
		{
			base.OnEnter(sm);
			audioSrc = GetComponent<AudioSource>();
			audioSrc.PlayOneShot(narrativeClip);
		}

		public override void OnUpdate(UltraStateMachine sm)
		{
			base.OnUpdate(sm);
			
			//Unstack if audio finished
			if (!audioSrc.isPlaying) Skip();

			if (Input.GetKeyDown(skipKey)) Skip();
		}

		//For button callbacks
		public void Skip() => gd.SafeSkip();
	}
} 