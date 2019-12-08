/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

using UnityEngine;

namespace StormRend.Systems.StateMachines
{
    public abstract class State : MonoBehaviour
    {
		[TextArea(0, 2), SerializeField] string Description = null;

		public virtual void OnEnter(UltraStateMachine sm) { }

		public virtual void OnUpdate(UltraStateMachine sm) { }

		public virtual void OnExit(UltraStateMachine sm) { }

        public virtual void OnCover(UltraStateMachine sm) {}

        public virtual void OnUncover(UltraStateMachine sm) {}
    }
}
