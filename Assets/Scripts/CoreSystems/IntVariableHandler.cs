using pokoro.BhaVE.Core.Variables;
using UnityEngine;
using UnityEngine.Events;

namespace StormRend.Variables.Utils
{
	/// <summary>
	/// Custom variable listener and limiter
	/// </summary>
	public class IntVariableHandler : MonoBehaviour
	{
		public enum LimitType { None, Clamp, WrapAround }

		[SerializeField] BhaveInt intVariable;

		[TextArea(0, 2), SerializeField, Space(5)] string description = "";

		[Header("Limits")]
		[SerializeField] LimitType limitType = LimitType.None;
		[SerializeField] int minLimit = 0;
		[SerializeField] int maxLimit = 1;

		[Header("Events")]
		public UnityEvent OnChanged;

		void OnValidate()
		{
			//Make sure: min <= max, max >= min
			if (minLimit > maxLimit) minLimit = maxLimit;
			if (maxLimit < minLimit) maxLimit = minLimit;
		}
		void OnEnable() => intVariable.onChanged += OnVarChanged;
		void OnDisable() => intVariable.onChanged -= OnVarChanged;

		void OnVarChanged()
		{
			//Invoke unity event
			OnChanged.Invoke();

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