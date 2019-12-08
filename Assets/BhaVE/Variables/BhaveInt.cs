/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

using System;
using UnityEngine;
namespace pokoro.BhaVE.Core.Variables
{
	[Serializable, CreateAssetMenu(menuName = "BhaVE/Variable/Int", fileName = "BhaveInt")]
	public sealed class BhaveInt : BHVar<int>
	{
		public enum LimitType { None, Clamp, WrapAround }
		[SerializeField] LimitType limitType = LimitType.None;
		[SerializeField] int minLimitInclusive = 0;
		[SerializeField] int maxLimitExclusive = 1;

		public override event Action onChanged = null;

		public override int value
		{
			get => _value;
			set
			{
				if (_value != value)
				{
					//Set
					_value = value;

					//Limit
					LimitValue();

					//Events
					onChanged?.Invoke();
					OnChanged?.Raise();
				}
			}
		}

		void OnValidate()
		{
			//Make sure: min <= max, max >= min
			if (minLimitInclusive > maxLimitExclusive) minLimitInclusive = maxLimitExclusive;
			if (maxLimitExclusive < minLimitInclusive) maxLimitExclusive = minLimitInclusive;
		}

		void LimitValue()
		{
			//Put limits on subject variable
			if (limitType == LimitType.None)
				return;

			//Over limit
			if (value > maxLimitExclusive)
			{
				switch (limitType)
				{
					case LimitType.Clamp:
						value = maxLimitExclusive;
						break;
					case LimitType.WrapAround:
						//Wraps around until variable is between min and max limits
						while (value < minLimitInclusive || value > maxLimitExclusive)
							value -= (maxLimitExclusive - (minLimitInclusive - 1));	//-1 to compensate for inclusive min value
						break;
				}
			}
			//Under limit
			else if (value < minLimitInclusive)
			{
				switch (limitType)
				{
					case LimitType.Clamp:
						value = minLimitInclusive;
						break;
					case LimitType.WrapAround:
						//Wraps around until variable is between min and max limits
						while (value < minLimitInclusive || value > maxLimitExclusive)
							value += (maxLimitExclusive - (minLimitInclusive + 1));
						break;
				}
			}
		}

		//Implicit convert definition to convert from int to BhaveInt
		//ie. Allows this:
		//BhaveInt bi = 10;		//Don't need bi.value = 10;
		// public static implicit operator BhaveInt(int rhs)
		// {
		// 	return new BhaveInt { value = rhs };
		// }

		//Implicit convert definition to convert from BhaveInt to Int
		//ie. Allows this:
		//BhaveInt bi = 5;
		// //int i = bi;	//Don't need int i = bi.value;
		// public static implicit operator int(BhaveInt self)
		// {
		// 	return self.value;
		// }
	}
}
