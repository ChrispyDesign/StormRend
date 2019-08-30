using System.Collections;
using System.Collections.Generic;
using StormRend.States.UI;
using StormRend.Systems.StateMachines;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace StormRend.States
{
    public class GameStateDirector : TurnBasedStackStateMachine
    {
        [Header("debug")]
        [SerializeField] bool on = false;

        //Should this handle pause menu?
        
        [Header("Game Pause")]
        [SerializeField] KeyCode pauseKey = KeyCode.Escape;
        [SerializeField] UIState pauseMenuState;

        void Start()
        {
            Assert.IsNotNull(pauseMenuState, "No pause menu state found!");
        }

        protected override void Update()
        {
            base.Update();

            HandlePauseMenu();
        }

        void HandlePauseMenu()
        {
            if (Input.GetKeyDown(pauseKey))
                Stack(pauseMenuState);
        }

        void OnSceneGUI()
        {
            if (on)
            {
                if (GUILayout.Button("PrevTurn"))
                {
                    PrevTurn();
                }
            }
        }
    }
}