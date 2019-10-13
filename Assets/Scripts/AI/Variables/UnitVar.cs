using pokoro.BhaVE.Core.Variables;
using StormRend.Defunct;
using UnityEngine;

namespace StormRend.Variables 
{ 
	[CreateAssetMenu(menuName = "StormRend/Variables/Unit", fileName = "UnitVar")]
	public class UnitVar : BhaveVar<xUnit>
	{
		public static implicit operator UnitVar(xUnit rhs)
		{
			return new UnitVar { value = rhs };
		}

		public static implicit operator xUnit(UnitVar self)
		{
			return self.value;
		}
   	}
}