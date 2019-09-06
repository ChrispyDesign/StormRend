using StormRend.Defunct;
using StormRend.Systems.StateMachines;

namespace StormRend.States
{
	public sealed class AllyTurnState : TurnState
	{
		//What should be here?
		// - Clear Undo
		// - Enable/Disable user input

		public override void OnEnter(UltraStateMachine sm)
		{
			base.OnEnter(sm);

			///------ Garbage code to be refactored  --------
			GameManager.singleton.GetCommandManager().ClearCommands();			//Clear undos
			// GameManager.singleton.GetPlayerController().enabled = true;		//Enable user control Update: Doesn't work
			//-----------------------------------------------------
		}

		public override void OnExit(UltraStateMachine sm)
		{
			base.OnExit(sm);

			///------ Garbage code to be refactored  --------
			// GameManager.singleton.GetPlayerController().enabled = false;	//Disable user control Update: Doesn't work
			//-----------------------------------------------------
		}
	}
}
