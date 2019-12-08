/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

using System;
using StormRend.Systems.StateMachines;
using UnityEngine;

namespace StormRend.States
{
	/// <summary>
	/// Automatically covers current state with specified objects on enter. Uncovers on exit
	/// </summary>
	public class CoverState : State
	{
		[Tooltip("Objects that will be activated to 'cover' the previous state")]
		[SerializeField] GameObject[] activateObjects = null;
		[Tooltip("Objects that will be deactivated on entering this state")]
		[SerializeField] GameObject[] deactivateObjects = null;

		///NOTE! All these methods must be called from override methods to preserve correct functionality
		public override void OnEnter(UltraStateMachine sm)
		{
			SetActivateObjects(true);
			SetDeactivateObjects(false);
		}
		public override void OnUncover(UltraStateMachine sm)
		{
			SetActivateObjects(true);
			SetDeactivateObjects(false);
		}
		public override void OnExit(UltraStateMachine sm)
		{
			SetActivateObjects(false);
			SetDeactivateObjects(true);
		}
		public override void OnCover(UltraStateMachine sm)
		{
			SetActivateObjects(false);
			SetDeactivateObjects(true);
		}

		void SetActivateObjects(bool active)
		{
			foreach (var i in activateObjects)
				i?.SetActive(active);
		}
		void SetDeactivateObjects(bool active)
		{
			foreach (var i in deactivateObjects)
				i?.SetActive(active);
		}
	}
}
