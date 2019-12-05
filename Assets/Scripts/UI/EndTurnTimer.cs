using System;
using UnityEngine;
using UnityEngine.UI;

namespace StormRend.UI
{
	/// <summary>
	/// Timer to briefly countdown the time before the turn automatically ends
	/// </summary>
	public class EndTurnTimer : MonoBehaviour
	{
		//Inspector
		[SerializeField] string format = @"ss\:ff";

		//Members
		Text text = null;

		void Start() => text = transform.GetChild(0).GetComponent<Text>();

		public void SetTime(float seconds)
		{
			TimeSpan counter = TimeSpan.FromSeconds(seconds);
			text.text = counter.ToString(format);
		}
	}
}