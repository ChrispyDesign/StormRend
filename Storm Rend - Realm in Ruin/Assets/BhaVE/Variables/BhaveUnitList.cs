using System.Collections.Generic;
using StormRend;
using UnityEngine;
namespace BhaVE.Variables
{
	[CreateAssetMenu(menuName = "StormRend/BhaVE/Variables/UnitList", fileName = "BhaveUnitList")]
	public sealed class BhaveUnitList : BhaveVar<List<Unit>>
	{
		public static implicit operator BhaveUnitList(List<Unit> rhs)
		{
			return new BhaveUnitList { value = rhs };
		}
		public static implicit operator List<Unit>(BhaveUnitList self)
		{
			return self.value;
		}
	}
}
