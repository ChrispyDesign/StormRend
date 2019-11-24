using System;
using System.Collections.Generic;
using pokoro.BhaVE.Core.Variables;
using pokoro.Patterns.Generic;
using StormRend.Abilities;
using StormRend.CameraSystem;
using StormRend.Enums;
using StormRend.MapSystems.Tiles;
using StormRend.States;
using StormRend.Systems.StateMachines;
using StormRend.Units;
using StormRend.Utility.Attributes;
using StormRend.Utility.Events;
using StormRend.Variables;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace StormRend.Systems
{
	public struct FrameEventData
	{
		public const int lmb = 0, rmb = 1;
		public bool leftClicked;
		public bool leftReleased;
		public bool rightClicked;
		public bool rightReleased;

		public void Refresh()
		{
			leftClicked = Input.GetMouseButtonDown(lmb);
			leftReleased = Input.GetMouseButtonUp(lmb);
			rightClicked = Input.GetMouseButtonDown(rmb);
			rightReleased = Input.GetMouseButtonUp(rmb);
		}
	}

	public partial class UserInputHandler : Singleton<UserInputHandler>
	{
		//Enums
		public enum Mode
		{
			Select,
			Move,
			Action,      //Cast spells, attack, summon, etc
		}

		//Inspector
        [SerializeField] BhaveInt glory = null;
		[Header("State")]
		[ReadOnlyField, SerializeField] TurnState currentTurnState = null;
		[Space(10), SerializeField] UnitVar _selectedUnitVar = null;
		[SerializeField] AbilityVar _selectedAbilityVar = null;

		[Header("Tile Colors")]
		[SerializeField] TileHighlightSetting startHighlight = null;
		[SerializeField] TileHighlightSetting hoverHighlight = null;
		[SerializeField] TileHighlightSetting moveHighlight = null;
		[SerializeField] TileHighlightSetting actionHighlight = null;
		[SerializeField] TileHighlightSetting targetHighlight = null;

		[Header("Camera")]
		[SerializeField] float cameraSmoothTime = 1.75f;
		[SerializeField] LayerMask raycastLayerMask = ~0;

		//Properties
		Mode mode
		{
			get
			{
				//If an ability is current selected then unit can perform ACTION
				if (isAbilitySelected && selectedAnimateUnit && selectedAnimateUnit.canAct)
					return Mode.Action;
				//If only unit selected and can move the unit can perform MOVE
				else if (isUnitSelected && selectedAnimateUnit && selectedAnimateUnit.canMove)
					return Mode.Move;
				//Unit not selected
				else
					return Mode.Select;
			}
		}
		public Unit selectedUnit
		{
			get => _selectedUnitVar.value;
			internal set => _selectedUnitVar.value = value;
		}
		public AnimateUnit selectedAnimateUnit => (selectedUnit != null) ? selectedUnit as AnimateUnit : null;
		public bool isUnitSelected => selectedUnit != null;
		//Ability selection
		public Ability selectedAbility
		{
			get => _selectedAbilityVar.value;
			internal set => _selectedAbilityVar.value = value;
		}
		public bool isAbilitySelected => selectedAbility != null;
		bool notEnoughTargetTilesSelected => targetTileStack.Count < selectedAbility.requiredTiles;

		//Events
		[Space(5)]
		[Tooltip("When a unit is successfully selected")] public UnitEvent onUnitSelected = null;		
		[Tooltip("When a unit is deselected")] public UnityEvent onUnitCleared = null;
		[Tooltip("When an ability is chosen")] public AbilityEvent onAbilitySelected = null;
		[Tooltip("When an ability is cleared")] public UnityEvent onAbilityCleared = null;
		[Tooltip("When a valid target tile is selected, adding it to the target stack")] public TileEvent onTargetTileAdd = null;
		[Tooltip("When an invalid tile is selected")] public UnityEvent onTargetTileInvalid = null;
		[Tooltip("When a tile is popped from the target stack ie. user right clicks")] public TileEvent onTargetTileCancel = null;
        [Tooltip("When an there's not enough glory to perform ability")] public UnityEvent onNotEnoughGlory = null;
		[Tooltip("When an ability is performed")] public AbilityEvent onAbilityPerformed = null;

		//Members
		FrameEventData e;   //The events that happened this frame
		CameraMover camMover = null;
		Camera cam = null;
		Stack<Tile> targetTileStack = new Stack<Tile>();
		public bool debug = false;
		bool isUnitHit = false;
		bool isTileHit = false;
		Unit interimUnit = null;
		Tile interimTile = null;
		bool isTileHitEmpty = false;
		GraphicRaycaster gr = null;
		List<RaycastResult> GUIhits = new List<RaycastResult>();
		List<Type> currentControllableUnitTypes = new List<Type>();		//Holds the list of types that can be controlled for this game turn

	#region Core
		void Start()
		{
			//Inits
			cam = MasterCamera.current.camera;
			camMover = MasterCamera.current.cameraMover;
			gr = FindObjectOfType<GraphicRaycaster>();	//On the one and only canvas
			selectedUnit = null;
			selectedAbility = null;

			//Asserts
			Debug.Assert(cam, "Camera could not be located!");
			Debug.Assert(camMover, "CameraMover could not be located!");
			Debug.Assert(_selectedUnitVar, "No Selected Unit SOV!");
			Debug.Assert(_selectedAbilityVar, "No Selected Ability SOV!");
			Debug.Assert(gr, "No graphics raycaster found!");
		}

		void Update()
		{
			ProcessEvents();
		}
	
		void ProcessEvents()
		{
			e.Refresh();	//Refresh all input events

			if (e.leftClicked)	//LEFT CLICKED
			{
				//Poll events
				isUnitHit = TryGetRaycast<Unit>(out interimUnit);
				if (!isUnitHit) isTileHit = TryGetRaycast<Tile>(out interimTile);
				if (isTileHit) isTileHitEmpty = !UnitRegistry.IsAnyUnitOnTile(interimTile);

				//LEFT CLICK ALWAYS
				if (isUnitHit)
				{
					//!!! This logic needs to run first otherwise the camera will move on final add target tile
					//Clicking on any unit will focus camera on it unless in action mode?
					if (mode != Mode.Action)
						camMover.Move(interimUnit, cameraSmoothTime);
				}

				switch (mode)
				{
					case Mode.Action:   //ACTION MODE
						if (isUnitHit)
							AddTargetTile(interimUnit);
						else if (isTileHit)					//Maybe this needs to be an else if so that only one add target tile gets passed through
							AddTargetTile(interimTile);
						break;

					case Mode.Move:     //MOVE MODE
						if (isTileHit && isTileHitEmpty)	//Restrict to empty tiles only
						{
							if (selectedAnimateUnit.Move(interimTile))	//Try Move unit
								camMover.Move(interimTile, cameraSmoothTime);	//If move successful then focus camera
						}
						goto case Mode.Select;	//Fall through

					case Mode.Select:     //SELECT MODE
						if (isUnitHit && currentControllableUnitTypes.Contains(interimUnit.GetType()))	//Filter controllable units
							SelectUnit(interimUnit as AnimateUnit);
						break;
				}

			}
			else if (e.rightReleased)	//RIGHT CLICK RELEASED
			{
				switch (mode)
				{
					case Mode.Action:	//ACTION MODE
						if (notEnoughTargetTilesSelected && targetTileStack.Count > 0)
							PopTargetTile();	//UNDO 1 TARGET TILE SELECT
						else
							ClearSelectedAbility();	//CLEAR ABILITY
						break;
					case Mode.Move:		//MOVE MODE
						ClearSelectedUnit();		//CLEAR UNIT
						break;
				}
			}
			else 	//HOVER
			{
				switch (mode)
				{
					case Mode.Move:		//MOVE
						//Poll events
						isTileHit = TryGetRaycast<Tile>(out interimTile); //!!! MAKE SURE THE RAYCAST LAYERS ARE CORRECTLY SET !!!
						if (isTileHit) isTileHitEmpty = !UnitRegistry.IsAnyUnitOnTile(interimTile);	//Check tile is empty

						//Move ghost on hover if the tile is empty
						if (isTileHit && isTileHitEmpty)
						{
							//MOVE GHOST
							selectedAnimateUnit.Move(interimTile, true);
						}
						break;
				}
			}
		}
		#endregion

	#region Callbacks
		public void OnStateChanged(State newState)
		{
			var newTurnState = newState as TurnState;

			//Clear highlights etc if current state changed
			if (currentTurnState != newTurnState)
			{
				ClearSelectedUnit();
				ClearSelectedAbility();
				ClearAllTileHighlights();
				
				//Set new turn state
				currentTurnState = newTurnState;

				//Setup new controllable units
				PopulateControllableUnitTypes();
			}
		}
	#endregion

	#region Assists
		//If T object hit then return true and output it
		bool TryGetRaycast<T>(out T hit) where T : MonoBehaviour
		{
			//If EventSystem.currentselectedGameObject is null then it's not over a GUI object
			if (EventSystem.current.currentSelectedGameObject)
			{
				// Debug.Log("Pointer over UI");
				hit = null;
				return false;
			}

			Ray ray = cam.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out RaycastHit hitInfo, 5000f, raycastLayerMask.value))
			{
				hit = hitInfo.collider.GetComponent<T>();
				return (hit != null) ? true : false;
			}
			hit = null;
			return false;
		}

		//Is there a better more reliable way of doing this?
		bool IsPointerOverGUIObject()
		{
			//Set up the new Pointer Event
			var pointerEventData = new PointerEventData(EventSystem.current);

			//Set the Pointer Event Position to that of the mouse position
			pointerEventData.position = Input.mousePosition;

			//Raycast using the Graphics Raycaster and mouse click position
			GUIhits.Clear();
			gr.Raycast(pointerEventData, GUIhits);

			return (GUIhits.Count > 0) ? true : false;
		}

		void PopulateControllableUnitTypes()
		{
			currentControllableUnitTypes.Clear();

			//Only populate if the current turn state allows it
			if (currentTurnState.hasUserControllableUnits)
			{
				//Allies
				if ((currentTurnState.unitType & TargetType.Allies) == TargetType.Allies)
					currentControllableUnitTypes.Add(typeof(AllyUnit));
				//Enemies
				if ((currentTurnState.unitType & TargetType.Enemies) == TargetType.Enemies)
					currentControllableUnitTypes.Add(typeof(EnemyUnit));
			}
		}

        public bool EnoughGlory()
		{
			if (glory)	//Null check
			{
				//Return whether or not there's enough glory available for current ability
				return glory.value >= selectedAbility.gloryCost;
			}
			Debug.LogWarning("No glory SOV allocated!");
			return false;
		}
	#endregion

	#region Debug
		void OnGUI()
		{
			if (!debug) return;
			
			GUILayout.Label("Glory: " + glory.value);

			GUILayout.Label("ActivityMode: " + mode);

			GUILayout.Label("is a unit hit?: " + isUnitHit);
			GUILayout.Label("is a tile hit?: " + isTileHit);

			GUILayout.Label("is a unit selected?: " + isUnitSelected);
			// if (_selectedAbility != null) 
			GUILayout.Label("Selected Unit: " + selectedUnit?.name);
			// if (_selectedAbilityVar.value != null) GUILayout.Label("Selected Unit: " + _selectedUnitVar.value.name);

			GUILayout.Label("is an ability selected?: " + isAbilitySelected);
			// if (_selectedAbility != null) 
			GUILayout.Label("Selected Ability: " + selectedAbility?.name);
			// if (_selectedAbilityVar.value != null) GUILayout.Label("Selected Ability: " + _selectedAbilityVar.value.name);

			GUILayout.Label("GUI hits count: " + GUIhits.Count);

			GUILayout.Label(string.Format("targetTileStack ({0}):", targetTileStack.Count));
			foreach (var t in targetTileStack)
				GUILayout.Label(t.name);
		}
	#endregion
	}
}

/*
Main: Start, Update, ProcessEvents, Onstatechanged
Sets: SelectUnit, SelectAbility, AddTargetTile, PopTargetTile, selectedUnitPeformAbility
TileHighlights: OnHovers, ShowMoveTiles, ShowTargetTiles
Clears: ClearSelectedUnit, ClearSelectedAbility, ClearSelectedUnitTileHighlights, ClearAllTileHighlights
Assists: TryGetRayRaycast, IsPointerOverGUIObject, EnoughGlory
Debugs: OnGUI
*/