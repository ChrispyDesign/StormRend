using pokoro.BhaVE.Core.Variables;
using StormRend.Units;
using UnityEngine;

namespace StormRend.Variables 
{ 
	[CreateAssetMenu(menuName = "StormRend/Variables/AnimateUnit", fileName = "AnimateUnitVar")]
	public class AnimateUnitVar : BhaveVar<AnimateUnit>
	{
		public static implicit operator AnimateUnitVar(AnimateUnit rhs)
		{
			return new AnimateUnitVar { value = rhs };
		}

		public static implicit operator AnimateUnit(AnimateUnitVar self)
		{
			return self.value;
		}
   	}
}