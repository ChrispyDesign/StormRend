using System;
using pokoro.BhaVE.Core.Variables;
using StormRend.Abilities;
using UnityEngine;

namespace StormRend.Variables
{
	[Serializable, CreateAssetMenu(menuName = "StormRend/Variables/AbilityVar", fileName = "AbilityVar")]
    public class AbilityVar : BhaveVar<Ability>
    {
		public override event Action onChanged;
		public override Ability value
		{
			get => _value;
			set
			{
				if (_value != value)
				{
					_value = value;
					OnChanged?.Raise();
					onChanged?.Invoke();
				}
			}
		}
    }
}