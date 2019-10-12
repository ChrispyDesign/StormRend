using System.Collections.Generic;
using pokoro.BhaVE.Core.Variables;
using UnityEngine;

namespace StormRend.Variables
{
	[CreateAssetMenu(menuName = "StormRend/Variables/UnitList", fileName = "UnitListVar")]
	public sealed class UnitListVar : BhaveVar<List<Unit>>
	{
		public override bool Equals(object other)
		{
			var otherUnitList = other as List<Unit>;

			//Valid check
			if (otherUnitList == null)
				throw new System.NullReferenceException("other unit list is null");

			//Size must match
			if (value.Count != otherUnitList.Count)
				return false;

			//Loop through this list and check if the values match up with the other list
			for (int i = 0; i < value.Count; i++)
			{
				if (value[i] != otherUnitList[i])
					return false;
			}
			return true;
		}
		public override int GetHashCode() => base.GetHashCode();

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
