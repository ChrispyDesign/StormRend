using pokoro.Patterns.Generic;
using StormRend.CameraSystem;
using StormRend.MapSystems.Tiles;
using StormRend.Systems.StateMachines;
using StormRend.Units;
using StormRend.Utility.Attributes;
using StormRend.Utility.Events;
using StormRend.Variables;
using UnityEngine;
using UnityEngine.Events;

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

namespace StormRend.Systems
{
	struct FrameEventData
	{
		public const int lmb = 0, rmb = 1;
		public bool leftClicked;
		public bool rightClicked;
		public bool rightClickUp;
		// public bool wasTileHit;
		// public bool wasUnitHit;
		// public Unit u;
		// public Tile t;
		public void Refresh()
		{
			leftClicked = Input.GetMouseButtonDown(lmb);
			rightClicked = Input.GetMouseButtonDown(rmb);
			rightClickUp = Input.GetMouseButtonUp(rmb);
			// wasUnitHit = UserInputHandler.current.Raycast<Unit>(out u);
			// wasTileHit = UserInputHandler.current.Raycast<Tile>(out t);
		}
	}
	public class UserInputHandler : MonoBehaviour
    {
		//Enums
        public enum ActivityMode 
		{ 
			Move, 	
			Action 		//Cast spells, attack, summon, etc
		}

		//Inspector
		[SerializeField] LayerMask raycastLayerMask;
		[Tooltip("A reference to the State object that is considered to be the player's state ie. AllyState")]
		[Space(5), SerializeField] State playersState;
		[Space(5), SerializeField] UnitVar _currentUnit;

		//Properties
		public Unit currentUnit 
		{
			get => _currentUnit.value;
			set => _currentUnit.value = value;
		}
		public bool isPlayersTurn { get; set; } = true;
		public bool unitIsSelected => currentUnit != null;

		//Events
		[Space(5)]
		public UnitEvent OnUnitSelectedChanged;
		public UnityEvent OnUnitDeselect;

		//Members
		FrameEventData e;	//The events that happenned this frame
		ActivityMode mode = ActivityMode.Move;
		CameraMover camMover;

		//Debug
		[Space] public bool debug;
		bool isUnitHit;
		Unit subjectUnit;

		#region Core
		void Awake()
		{
			//Find
			camMover = MasterCamera.current.linkedCamera.GetComponent<CameraMover>();
		}
		void Start()
		{
			// Debug.Assert(currentUnit, "Selected Unit SOV not assigned!");
			Debug.Assert(camMover, "CameraMover could not be located!");
		}

		void Update()
		{
			ProcessEvents();
		}

		void ProcessEvents()
		{
			//Refresh event
			e.Refresh();
			isUnitHit = Raycast<Unit>(out subjectUnit);
			// tileHit = Raycast<Tile>(out Tile t);

			//PLAYERS TURN
			if (isPlayersTurn)
			{
				//UNIT RAY HIT
				if (isUnitHit)
				{
					//A UNIT SELECTED
					if (unitIsSelected) { }
					//NO UNIT SELECTED
					else { }

					//LEFT CLICK
					if (e.leftClicked)
					{
					}
				}
				// //TILE HIT
				// else if (tileHit)
				// {
				// 	//A UNIT SELECTED
				// 	if (unitIsSelected) { }
				// 	//NO UNIT SELECTED
				// 	else { }
				// }
			}

			//ALWAYS RUN
			//A UNIT SELECTED
			if (unitIsSelected) { }
			//NO UNIT SELECTED
			else { }

			//Focus Camera
			if (e.leftClicked)
			{
				if (isUnitHit)
				{
					SelectUnit(subjectUnit);
					FocusCamera(subjectUnit);
				}
			}

			if (e.rightClickUp)
				DeselectAll();
		}

		void OnGUI()
		{
			if (!debug) return;
			GUILayout.Label("is a unit hit?: " + isUnitHit);
			GUILayout.Label("is a unit selected?: " + unitIsSelected);
			GUILayout.Label("Selected Unit: " + currentUnit?.name);
		}
	#endregion

	#region Assists
		//Moves to and looks at passed in subject
		void FocusCamera(Unit u, float smoothTime = 1)
		{
			camMover.MoveTo(u, smoothTime);
		}
		void FocusCamera(Tile t, float smoothTime = 1)
		{
			camMover.MoveTo(t.transform.position, smoothTime);
		}
		//Sets the selected unit
		void SelectUnit(Unit u)
		{
			OnUnitSelectedChanged.Invoke(u);

			//Set the selected unt
			currentUnit = u;

			//Update tile highlights based on mode of activity
		}
		//Deselects the unit
		void DeselectAll()
		{
			OnUnitDeselect.Invoke();

			currentUnit = null;
		}

		internal bool Raycast<T>(out T hit) where T : MonoBehaviour
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out RaycastHit hitInfo, float.PositiveInfinity, raycastLayerMask.value))
			{
				// Debug.Assert(hitInfo.collider, "hitInfo.collider is null");
				hit = hitInfo.collider.GetComponent<T>();
				// Debug.Assert(hit, "hit " + hit.GetType().Name + " was null");
				
				return (hit != null) ? true : false;
			}
			hit = null;
			return false;
		}
	#endregion
    }
}