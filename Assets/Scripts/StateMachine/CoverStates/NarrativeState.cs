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

		//Members
		GameDirector gd = null;
		PlayableDirector pd = null;

		void OnEnable()
		{
			gd = GameDirector.current;
			pd = GetComponent<PlayableDirector>();

			pd.paused += SkipRelay;
		}
		void OnDisable()
		{
			pd.paused -= SkipRelay;
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
		public void Skip() => gd.SafeSkip();
	}
} 