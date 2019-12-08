/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

using StormRend.Systems.StateMachines;
using UnityEngine;

namespace StormRend.States
{
	/// <summary>
	/// Automatically activates specified objects on enter without covering. Deactivates on exit
	/// </summary>
	public class OnState : State
	{
		[Tooltip("Objects that will be activated on state enter")]
		[SerializeField] GameObject[] activatingObjects = null;

		public override void OnEnter(UltraStateMachine sm) => SetObjectsActive(true);
		public override void OnExit(UltraStateMachine sm) => SetObjectsActive(false);

		void SetObjectsActive(bool active)
		{
			foreach (var i in activatingObjects)
				i?.SetActive(active);
		}
	}
}