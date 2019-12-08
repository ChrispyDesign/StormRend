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
using UnityEngine.Events;

namespace StormRend.Variables.Utils
{
	/// <summary>
	/// Custom variable listener and limiter
	/// </summary>
	public class GloryTester : MonoBehaviour
	{
		[SerializeField] BhaveInt glory = null;

		[TextArea(0, 2), SerializeField, Space(5)] string description = null;

		[Space]
		[SerializeField] bool debug = false;
		[SerializeField] KeyCode increaseKey = KeyCode.RightBracket;
		[SerializeField] KeyCode decreaseKey = KeyCode.LeftBracket;

		void Start() =>	glory.value = 0;	//Reset at startup

		void Update()
		{
			if (!debug) return;

			if (Input.GetKeyDown(increaseKey)) ++glory.value;
			if (Input.GetKeyDown(decreaseKey)) --glory.value;
		}
		void OnGUI()
		{
			if (!debug) return;

			GUILayout.Label("GLORY: " + glory?.value);
		}
	}
}