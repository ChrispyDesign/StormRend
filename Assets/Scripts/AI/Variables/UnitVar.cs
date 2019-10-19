using System;
using pokoro.BhaVE.Core.Variables;
using StormRend.Units;
using UnityEngine;

namespace StormRend.Variables
{
    [Serializable, CreateAssetMenu(menuName = "StormRend/Variables/Unit", fileName = "UnitVar")]
	public sealed class UnitVar : BhaveVar<Unit>
	{
		public override Unit value
		{
			get => _value;
			set => _value = value;		//NOTE currently this means onchange events don't work
			// {
			// 	if (_value != value)
			// 	{
			// 		_value = value;
			// 		onChanged();
			// 	}
			// }
		}

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