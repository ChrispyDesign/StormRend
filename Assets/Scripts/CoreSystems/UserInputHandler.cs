using pokoro.Patterns.Generic;
using StormRend.Variables;
using UnityEngine;
using UnityEngine.EventSystems;

namespace StormRend.Systems
{
    public class UserInputHandler : Singleton<UserInputHandler>
    {
        public enum Mode
		{
			UserTurn,
			AITurn,

            Disabled,
            Select,
            Action,
		}
		
		/*Brainstorm:
        ------ UserInputHandler functionality
        if a unit is selected
            go into action mode (allows the user to move or perform abilities with the unit)
            show the appropriate ui for that selected unit
        else if no unit is selected
            go into select mode


		------- Possible cast and move tiles
		Q Where should the possible cast and move tiles be stored?
		A Possible Cast Tiles should be stored on each Ability SO and updated every time
		A Possible Move Tiles should be stored on each Unit 
		A Possible 

		Q When should the possible move tiles be calculated?
		A At the begginning of the turn, for each unit
			since the unit can only move once per turn

		Q When should the possible cast tiles be calculated?
		A Everytime an ability select button is clicked, the input handler will help to calculate
		*/

		//Inspector
        [SerializeField] PhysicsRaycaster raycaster;
		EventSystem eventSystem;

		
		[Tooltip("Starting game mode")]
		[SerializeField] Mode mode = Mode.UserTurn;

		[Header("SOVs")]
		[SerializeField] AnimateUnitVar selectedUnit;
		[SerializeField] TileVar hoveredTile;

		//Members


	#region Core
		void Awake()
		{
			eventSystem = EventSystem.current;
		}

		void Update()
		{
            // raycaster.Raycast()
			
		}
	#endregion
    }
}