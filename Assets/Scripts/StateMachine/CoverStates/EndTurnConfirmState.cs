/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

namespace StormRend.States
{
	/// <summary>
	/// Contains logic to handle whether or not to go to next turn or not
	/// </summary>
	public class EndTurnConfirmState : CoverState
	{
		//1. if all actions are used up then:
		// - Unstack this state
		// - safely Go to the next turn state

		//2. If there are still actions to be used
		// - Stack and enter this state as normal, displaying the end confirmation dialog

		//3. Go back => Button.onClick => usm.UnStack()

		//4. Next Turn Anyways (forced)
		// - Unstack this state. Unstacking will ALWAYS stop the end turn timer
		// - Go to the next turn state
	}
}