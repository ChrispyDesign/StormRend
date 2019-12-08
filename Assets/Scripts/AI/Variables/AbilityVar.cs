/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

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