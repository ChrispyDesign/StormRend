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
using UnityEngine.UI;

namespace StormRend.UI
{
	[RequireComponent(typeof(Toggle))]
	public sealed class ToggleUpdater : UIUpdater
	{
		[SerializeField] BhaveBool sov = null;
		Toggle toggle = null;

		void Awake()
		{
			toggle = GetComponent<Toggle>();
			Debug.Assert(sov != null, "SOV not found!");
		}

		void OnEnable() => toggle.isOn = sov.value;
	}
}