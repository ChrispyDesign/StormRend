/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

using pokoro.BhaVE.Core.Variables;
using UnityEngine;

namespace StormRend.Variables.Utils
{
	/// <summary>
	/// BhaveInt variable limiter
	/// </summary>
	public class BhaveIntLimiter : MonoBehaviour
	{
		public enum LimitType { None, Clamp, WrapAround }

		[SerializeField] BhaveInt intVariable = null;

		[SerializeField, TextArea, Space(5)] string description = null;

		[Header("Limits")]
		[SerializeField] LimitType limitType = LimitType.None;
		[SerializeField] int minLimit = 0;
		[SerializeField] int maxLimit = 1;

		void OnValidate()
		{
			//Make sure: min <= max, max >= min
			if (minLimit > maxLimit) minLimit = maxLimit;
			if (maxLimit < minLimit) maxLimit = minLimit;
		}
		void OnEnable() => intVariable.onChanged += ApplyLimitsOnChanged;
		void OnDisable() => intVariable.onChanged -= ApplyLimitsOnChanged;

		void ApplyLimitsOnChanged()
		{
			//Put limits on subject variable
			if (limitType == LimitType.None)
				return;

			//Over limit
			if (intVariable.value > maxLimit)
			{
				switch (limitType)
				{
					case LimitType.Clamp:
						intVariable.value = maxLimit;
						break;
					case LimitType.WrapAround:
						//Wraps around until variable is between min and max limits
						while (intVariable.value < minLimit || intVariable.value > maxLimit)
							intVariable.value -= (maxLimit - minLimit);
						break;
				}
			}
			//Under limit
			else if (intVariable.value < minLimit)
			{
				switch (limitType)
				{
					case LimitType.Clamp:
						intVariable.value = minLimit;
						break;
					case LimitType.WrapAround:
						//Wraps around until variable is between min and max limits
						while (intVariable.value < minLimit || intVariable.value > maxLimit)
							intVariable.value += (maxLimit - minLimit);
						break;
				}
			}
		}
	}
}