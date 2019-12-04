using System.Collections;
using pokoro.BhaVE.Core.Variables;
using UnityEngine;
using UnityEngine.UI;

namespace StormRend.UI
{
	public class Meter : MonoBehaviour
	{
		//Inspector
		[SerializeField] protected BhaveInt SOV = null;
		[SerializeField] protected Image[] meterPips = null;
		[Range(0f, 2f), SerializeField] float fillDuration = 0.3f; //seconds

		//Members
		protected int internalValue
		{
			get => _internalValue;
			set => _internalValue = Mathf.Clamp(value, 0, 5);
		}
		protected bool increase = false;
		protected bool decrease = false;
		int _internalValue = 0;

		[SerializeField] protected bool debug = false;

		void Awake()
		{
			Debug.Assert(SOV, "No SOV found!");
		}

		void Start()
		{
			//Reset internal value and fills
			internalValue = 0;
			foreach (var pip in meterPips)
				pip.fillAmount = 0f;

			SOV.onChanged += OnChange;
		}
		void OnDestroy() => SOV.onChanged -= OnChange;

		public virtual void OnChange() => StartCoroutine(AutoAdjustMeter());

		protected IEnumerator AutoAdjustMeter()
		{
			int difference = SOV.value - internalValue;

			//Increase
			if (difference > 0)
			{
				while (internalValue < SOV.value && !increase)
				{
					increase = true;
					yield return FillOnePip();
					internalValue++;
					increase = false;
				}
			}
			//Decrease
			else if (difference < 0)
			{
				while (internalValue > SOV.value && !decrease)
				{
					decrease = true;
					yield return UnfillOnePip();
					internalValue--;
					decrease = false;
				}
			}
			//No change do nothing
			else
			{
				Debug.LogWarning("No change in meter");
			}
		}

		/// <summary>
		/// Fill a pip starting from the current internal pip index
		/// </summary>
		IEnumerator FillOnePip()
		{
			//Cancel if out of range
			if (internalValue >= meterPips.Length) yield break; 

			//Adjust pip
			float amount = 0f;
			float rate = 1f / fillDuration;
			while (amount < 1f)
			{
				amount += rate * Time.deltaTime;
				meterPips[internalValue].fillAmount = amount;
				yield return null;
			}
			//Account for any micro timing errors
			meterPips[internalValue].fillAmount = 1f;   
		}

		/// <summary>
		/// Unfills a pip
		/// </summary>
		/// <returns></returns>
		IEnumerator UnfillOnePip()
		{
			if (internalValue <= 0) yield break; 
			float amount = 1f;
			float rate = 1f / fillDuration;
			while (amount > 0f)
			{
				amount -= rate * Time.deltaTime;
				meterPips[internalValue - 1].fillAmount = amount;
				yield return null;
			}
			meterPips[internalValue - 1].fillAmount = 0f;
		}
	}
}
