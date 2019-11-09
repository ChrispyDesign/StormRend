using StormRend.Abilities.Effects;
using UnityEngine;

namespace StormRend.Anim.EventHandlers 
{ 
	/// <summary>
	/// Hardcoded 
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

			//TEMP
			au.Act<RefreshEffect>();
		}
   	}
}