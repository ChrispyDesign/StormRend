using System;
using TMPro;
using UnityEngine;

namespace StormRend.UI
{
	/// <summary>
	/// Timer to briefly countdown the time before the turn automatically ends
	/// </summary>
	public class TurnEndingCountdown : MonoBehaviour
	{
		TextMeshProUGUI text = null;

		void Awake()
		{
			text = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
		}

		public void SetTime(float seconds)
		{
			TimeSpan counter = TimeSpan.FromSeconds(seconds);
			text.text = counter.ToString(@"ss\:ff");
		}
	}
}