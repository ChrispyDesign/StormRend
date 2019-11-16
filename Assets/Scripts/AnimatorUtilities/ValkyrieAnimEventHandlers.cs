using StormRend.Abilities.Effects;
using UnityEngine;

namespace StormRend.Anim.EventHandlers
{
	/// <summary>
	/// Hardcoded Valkyrie callbacks
	/// </summary>
	public class ValkyrieAnimEventHandlers : UnitAnimEventHandlers
	{
		public void PerformTeleport()
		{
			animateUnit.Act<TeleportEffect>();
		}

		public void PerformPush()
		{
			animateUnit.Act<PushEffect>();

			animateUnit.Act<RefreshEffect>();    //TODO Temporary. Delete later
		}
   	}
}