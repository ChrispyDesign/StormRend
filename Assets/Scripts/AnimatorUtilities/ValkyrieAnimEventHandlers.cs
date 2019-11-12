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
			au.Act<TeleportEffect>();
		}

		public void PerformPush()
		{
			au.Act<PushEffect>();

			au.Act<RefreshEffect>();    //TODO Temporary. Delete later
		}
   	}
}