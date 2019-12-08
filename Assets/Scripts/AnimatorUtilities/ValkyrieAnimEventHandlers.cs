/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

using StormRend.Abilities.Effects;
using UnityEngine;

namespace StormRend.Anim.EventHandlers
{
	/// <summary>
	/// Hardcoded Valkyrie callbacks
	/// </summary>
	public class ValkyrieAnimEventHandlers : UnitAnimEventHandlers
	{
		/// <summary>
		/// Call at the end of the jump animation
		/// </summary>
		public void PerformJump()
		{
			animateUnit?.Act<TeleportEffect>();
		}

		/// <summary>
		/// Perform actions at the point where lightfall lands ie. push
		/// </summary>
		public void PerformLand()
		{
			//Covers all 3 levels of lightfall
			animateUnit?.Act<PushEffect>();
			animateUnit?.Act<RefreshEffect>();
			animateUnit?.Act<ProtectEffect>();
		}
   	}
}