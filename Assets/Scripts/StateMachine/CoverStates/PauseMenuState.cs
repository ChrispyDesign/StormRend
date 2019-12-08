/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

using UnityEngine;
using StormRend.Systems.StateMachines;

namespace StormRend.States
{
    /// <summary>
    /// * Auto handles game timescale changes
    /// </summary>
    public class PauseMenuState : CoverState
    {
        [Space(10), SerializeField] float pauseTimeScale = 0.2f;

        public override void OnEnter(UltraStateMachine sm)
        {
            base.OnEnter(sm);

            Time.timeScale = pauseTimeScale;
        }

        public override void OnExit(UltraStateMachine sm)
        {
            base.OnExit(sm);

            Time.timeScale = 1f;
        }
    }
}