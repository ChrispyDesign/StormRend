using System.Collections.Generic;
using pokoro.BhaVE.Core.Variables;
using UnityEngine;

namespace StormRend.Variables
{
	[CreateAssetMenu(menuName = "StormRend/Variables/UnitList", fileName = "UnitListVar")]
	public sealed class UnitListVar : BhaveVar<List<Unit>>
	{
        public static implicit operator UnitListVar(List<Unit> rhs)
		{
			return new UnitListVar { value = rhs };
		}
		public static implicit operator List<Unit>(UnitListVar self)
		{
			return self.value;
		}
	}
}
