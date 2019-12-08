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

		void Start() => this.text = transform.GetChild(0).GetComponent<Text>();

		public void SetTime(float seconds)
		{
			TimeSpan counter = TimeSpan.FromSeconds(seconds);

			if (!this.text) Start();

			this.text.text = counter.ToString(format);
		}
	}
}