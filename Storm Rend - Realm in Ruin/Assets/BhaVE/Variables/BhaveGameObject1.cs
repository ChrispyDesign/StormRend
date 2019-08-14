using StormRend;
using UnityEngine;
namespace BhaVE.Variables
{
	[CreateAssetMenu(menuName = "StormRend/BhaVe/Variables/UnitList", fileName = "UnitList")]
	public sealed class UnitList : BhaveVar<Unit[]>
	{
		public static implicit operator UnitList(Unit[] rhs)
		{
			return new UnitList { value = rhs };
		}
		public static implicit operator Unit[](UnitList self)
		{
			return self.value;
		}
	}
}
