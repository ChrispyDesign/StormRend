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