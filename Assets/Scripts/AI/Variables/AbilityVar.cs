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
				//NOTE: this apparently prevents null ref exception on startup. Not 100% trustworthy though
				if (value && _value != value)
				{
					_value = value;
					OnChanged?.Raise();
					onChanged?.Invoke();
				}
				else
					_value = value;
			}
		}
    }
}