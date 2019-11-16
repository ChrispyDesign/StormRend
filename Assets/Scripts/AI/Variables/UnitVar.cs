using System;
using pokoro.BhaVE.Core.Variables;
using StormRend.Units;
using UnityEngine;

namespace StormRend.Variables
{
    [Serializable, CreateAssetMenu(menuName = "StormRend/Variables/Unit", fileName = "UnitVar")]
	public sealed class UnitVar : BhaveVar<Unit>
	{
		public override event Action onChanged;
		public override Unit value
		{
			get => _value;
			set
			{
				//NOTE: this apparently prevents null ref exception on startup. Not 100% trustworthy though
				if (value && _value != value)
				{
					_value = value;
					OnChanged?.Raise();
					onChanged?.Invoke();
				}
				//If the value is null just set the value to
				else
					_value = value;
			}
		}
   	}
}