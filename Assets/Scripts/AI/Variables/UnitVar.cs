using pokoro.BhaVE.Core.Variables;
using StormRend.Units;
using UnityEngine;

namespace StormRend.Variables
{
    [CreateAssetMenu(menuName = "StormRend/Variables/Unit", fileName = "UnitVar")]
	public class UnitVar : BhaveVar<Unit>
	{
		public static implicit operator UnitVar(Unit rhs)
		{
			return new UnitVar { value = rhs };
		}

		public static implicit operator Unit(UnitVar self)
		{
			return self.value;
		}
   	}
}