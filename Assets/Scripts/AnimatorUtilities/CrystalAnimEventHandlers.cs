using StormRend.Units;
using UnityEngine;

namespace StormRend.Anim.EventHandlers
{
	public class CrystalAnimEventHandlers : UnitAnimEventHandlers
	{
		CrystalUnit crystal = null;
		void Start()
		{
			crystal = unit as CrystalUnit;
		}
		public void Explode()
		{
			crystal?.Explode();
		}
	}
}