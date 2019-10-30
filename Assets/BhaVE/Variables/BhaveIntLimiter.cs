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

		[SerializeField] BhaveInt intVariable;

		[SerializeField, TextArea, Space(5)] string description = "";

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
			if (intVariable > maxLimit)
			{
				switch (limitType)
				{
					case LimitType.Clamp:
						intVariable = maxLimit;
						break;
					case LimitType.WrapAround:
						//Wraps around until variable is between min and max limits
						while (intVariable < minLimit || intVariable > maxLimit)
							intVariable -= (maxLimit - minLimit);
						break;
				}
			}
			//Under limit
			else if (intVariable < minLimit)
			{
				switch (limitType)
				{
					case LimitType.Clamp:
						intVariable = minLimit;
						break;
					case LimitType.WrapAround:
						//Wraps around until variable is between min and max limits
						while (intVariable < minLimit || intVariable > maxLimit)
							intVariable += (maxLimit - minLimit);
						break;
				}
			}
		}
	}
}