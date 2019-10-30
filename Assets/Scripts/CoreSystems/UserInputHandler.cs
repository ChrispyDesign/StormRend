using System;
using StormRend.Abilities;
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

------ Gameplay
Moving Unit:
- Click on Unit
	- Set unit as current/active unit
	- 


Q How does the player do a spell/attack?
A The player clicks on a unit
	- UserInput will set selected unit
	- the camera will focus on unit
	- AbilityPanel updates AbilitySelectButtons unit's internal abilities
	- If player selects an ability:
		- AbilityPanel to send ability assigned to that button back to UserInput
		- UserInput sets selected ability
		- Tiles will update to show cast area based on chosen/subject/selected ability
	- If clicks on one of the tiles it will  

AbilityPanel
+ UpdateButtons(AnimateUnit unit)
{
	
}

UserInputHandler
+ OnAbilityChanged(Ability ability)
	SetAbility(ability)

Q How


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
			Action      //Cast spells, attack, summon, etc
		}

		//Inspector
		[SerializeField] LayerMask raycastLayerMask;

		[Tooltip("A reference to the State object that is considered to be the player's state ie. AllyState")]
		[Space(10), SerializeField] State playersState;

		[Space(10), SerializeField] UnitVar _activeUnit;
		[ReadOnlyField, SerializeField] Ability _activeAbility;

		[Header("Tile Highlight Colors")]
		[SerializeField] TileHighlightColor moveHighlight;
		[SerializeField] TileHighlightColor actionHighlight;


		//Properties
		public Unit activeUnit
		{
			get => _activeUnit.value;
			internal set => _activeUnit.value = value;
		}
		public Ability activeAbility
		{
			get => _activeAbility;
			internal set => _activeAbility = value;
		}
		public bool isPlayersTurn { get; set; } = true;
		public bool unitIsActive => activeUnit != null;

		//Events
		[Space(5)]
		public UnitEvent OnActiveUnitChanged;
		public UnityEvent OnActiveUnitCleared;

		//Members
		FrameEventData e;   //The events that happenned this frame
		ActivityMode mode = ActivityMode.Move;
		CameraMover camMover;

		//Debug
		public bool debug;
		bool unitHit;
		bool tileHit;
		Unit interimUnit;
		Tile interimTile;

		#region Core
		void Awake()
		{
			//Find
			camMover = MasterCamera.current.linkedCamera.GetComponent<CameraMover>();
		}
		void Start()
		{
			//Inits
			_activeUnit.value = null;

			//Asserts
			Debug.Assert(camMover, "CameraMover could not be located!");
		}

		void Update()
		{
			ProcessEvents();
		}

		void ProcessEvents()
		{
			e.Refresh();	//Refresh all input events

			if (e.leftClicked)
			{
				unitHit = Raycast<Unit>(out interimUnit);
				tileHit = Raycast<Tile>(out interimTile);

				if (isPlayersTurn) 
				{
					//Can only select units when it's the player's turn
					if (unitHit) SetActiveUnit(interimUnit);
				}

				if (unitHit)
				{
					//Clicking on a unit will ALWAYS focus camera on it
					FocusCamera(interimUnit);
				}
			}
			else if (e.rightClickUp)
			{
				ClearActiveUnit();
			}
		}

		void OnGUI()
		{
			if (!debug) return;
			GUILayout.Label("is a unit hit?: " + unitHit);
			GUILayout.Label("is a unit selected?: " + unitIsActive);
			GUILayout.Label("Selected Unit: " + _activeUnit?.value?.name);
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
		public void SetActiveUnit(Unit u)
		{
			OnActiveUnitChanged.Invoke(u);	//ie. Update UI, Play sounds, 

			//Set the selected unt
			activeUnit = u;

			//Update tile highlights based on mode of activity
			switch (mode)
			{
				case ActivityMode.Move:
					// UpdateMoveTiles(u);
					break;
				case ActivityMode.Action:
					// UpdateActionTiles(u);
					break;
			}
		}

		internal void UpdateMoveTiles(Unit u)
		{
			//NOTE: Active unit's MOVE tiles should be refreshed each turn
			
			//Only animate units 
			var au = u as AnimateUnit;
			if (!au) return;

			//Make sure there are tiles to highlight
			if (au.possibleMoveTiles == null)
				au.CalculateMoveTiles();

			//Highlight
			foreach (var t in au.possibleMoveTiles)
			{
				t.highlight.SetColor(moveHighlight);
				// t.highlight.SetColor(Tile.TryGetHighlightColor("Move"));
			}
		}

		internal void UpdateActionTiles(Unit u)
		{
			//NOTE: Active unit's MOVE tiles should be refreshed each OnAbilityChanged

			//Only animate units have possible 
			var au = u as AnimateUnit;
			if (!au) return;

			//Make sure there are tiles to highlight
			if (au.possibleActionTiles == null)
				activeAbility.CalculateActionableTiles(au);

			//Highlight
			foreach (var t in au.possibleActionTiles)
			{
				t.highlight.SetColor(actionHighlight);
				// t.highlight.SetColor(Tile.TryGetHighlightColor("Action"));
			}
		}

		//Deselects the unit
		void ClearActiveUnit()
		{
			OnActiveUnitCleared.Invoke();

			activeUnit = null;
		}

		public void ChangeAbility(Ability a)
		{
			//Set active ability
			activeAbility = a;

			//Update active unit's action tiles
			var au = activeUnit as AnimateUnit;
			au.possibleActionTiles = activeAbility.CalculateActionableTiles(au);
		}

		internal bool Raycast<T>(out T hit) where T : MonoBehaviour
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out RaycastHit hitInfo, float.PositiveInfinity, raycastLayerMask.value))
			{
				hit = hitInfo.collider.GetComponent<T>();
				return (hit != null) ? true : false;
			}
			hit = null;
			return false;
		}
		#endregion
	}
}