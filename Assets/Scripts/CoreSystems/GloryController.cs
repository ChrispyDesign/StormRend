using pokoro.BhaVE.Core.Variables;
using UnityEngine;
using UnityEngine.Events;

namespace StormRend.Variables.Utils
{
	/// <summary>
	/// Custom variable listener and limiter
	/// </summary>
	public class GloryController : MonoBehaviour
	{
		public enum LimitType { None, Clamp, WrapAround }

		[SerializeField] BhaveInt glory = null;

		[TextArea(0, 2), SerializeField, Space(5)] string description = null;

		[Header("Limits")]
		[SerializeField] LimitType limitType = LimitType.None;
		[SerializeField] int minLimit = 0;
		[SerializeField] int maxLimit = 1;

		[Header("Events")]
		public UnityEvent OnChanged = null;

		void OnValidate()
		{
			//Make sure: min <= max, max >= min
			if (minLimit > maxLimit) minLimit = maxLimit;
			if (maxLimit < minLimit) maxLimit = minLimit;
		}
		void OnEnable() => glory.onChanged += OnVarChanged;
		void OnDisable() => glory.onChanged -= OnVarChanged;

		void Start() =>	glory.value = 0;	//Reset at startup

		void OnVarChanged()
		{
			//Invoke unity event
			OnChanged.Invoke();

			//Put limits on subject variable
			if (limitType == LimitType.None)
				return;

			//Over limit
			if (glory.value > maxLimit)
			{
				switch (limitType)
				{
					case LimitType.Clamp:
						glory.value = maxLimit;
						break;
					case LimitType.WrapAround:
						//Wraps around until variable is between min and max limits
						while (glory.value < minLimit || glory.value > maxLimit)
							glory.value -= (maxLimit - minLimit);
						break;
				}
			}
			//Under limit
			else if (glory.value < minLimit)
			{
				switch (limitType)
				{
					case LimitType.Clamp:
						glory.value = minLimit;
						break;
					case LimitType.WrapAround:
						//Wraps around until variable is between min and max limits
						while (glory.value < minLimit || glory.value > maxLimit)
							glory.value += (maxLimit - minLimit);
						break;
				}
			}
		}
	}
}