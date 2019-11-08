using System;
using System.Collections.Generic;
using pokoro.Patterns.Generic;
using StormRend.Abilities;
using StormRend.CameraSystem;
using StormRend.Enums;
using StormRend.MapSystems;
using StormRend.MapSystems.Tiles;
using StormRend.States;
using StormRend.Systems.StateMachines;
using StormRend.Units;
using StormRend.Utility;
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

	public class UserInputHandler : Singleton<UserInputHandler>
	{
		//Enums
		public enum Mode
		{
			Select,
			Move,
			Action,      //Cast spells, attack, summon, etc
		}

		//Inspector
		// [Tooltip("A reference to the State object that is considered to be the player's state ie. AllyState")]
		[Header("State")]
		[ReadOnlyField, SerializeField] TurnState currentTurnState = null;
		[Space(10), SerializeField] UnitVar _selectedUnitVar = null;
		[SerializeField] AbilityVar _selectedAbilityVar = null;

		[Header("Tile Colors")]
		[SerializeField] TileHighlightColor moveHighlight = null;
		[SerializeField] TileHighlightColor actionHighlight = null;

		[Header("Camera")]
		[SerializeField] float cameraSmoothTime = 1.75f;
		[SerializeField] LayerMask raycastLayerMask;

		//Properties
		Mode mode
		{
			get
			{
				//If an ability is current selected then unit can perform ACTION
				if (isAbilitySelected && selectedAnimateUnit.canAct)
					return Mode.Action;
				//If only unit selected and can move the unit can perform MOVE
				else if (isUnitSelected && selectedAnimateUnit.canMove)
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
		public UnitEvent onUnitChanged;
		public UnityEvent onUnitCleared;
		public AbilityEvent onAbilityChanged;
		public AbilityEvent onAbilityPerformed;
		public UnityEvent onAbilityCleared;

		//Members
		FrameEventData e;   //The events that happened this frame
		CameraMover camMover;
		Camera cam;
		Stack<Tile> targetTileStack = new Stack<Tile>();
		public bool debug;
		bool isUnitHit;
		bool isTileHit;
		Unit interimUnit;
		Tile interimTile;
		bool isTileHitEmpty;
		GraphicRaycaster gr;
		List<RaycastResult> GUIhits = new List<RaycastResult>();
		List<Type> currentControllableUnitTypes = new List<Type>();		//Holds the list of types that can be controlled for this game turn

	// #region Callback Registration
	// 	void OnEnable()
	// 	{
	// 		_selectedUnitVar.onChanged += OnSelectedUnitChanged;
	// 		_selectedAbilityVar.onChanged += OnSelectedAbilityChanged;
	// 	}
	// 	void OnDisable()
	// 	{
	// 		_selectedUnitVar.onChanged -= OnSelectedUnitChanged;
	// 		_selectedAbilityVar.onChanged -= OnSelectedAbilityChanged;
	// 	}
	// 	void OnSelectedUnitChanged()
	// 	{
	// 		//
	// 		if (selectedUnit)
	// 			SelectUnit(selectedUnit as AnimateUnit);
	// 		else
	// 			ClearSelectedUnit();
	// 	}
	// 	void OnSelectedAbilityChanged()
	// 	{
	// 		if (selectedAbility)
	// 			SelectAbility(selectedAbility);
	// 		else
	// 			ClearSelectedAbility();
	// 	}
	// #endregion

	#region Core
		void Start()
		{
			//Inits
			cam = MasterCamera.current.camera;
			camMover = cam.GetComponent<CameraMover>();
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
			// tMoveByV2ITest();
		}

		// void tMoveByV2ITest()	//Move by Vector 2 Int Test
		// {
		// 	Vector2Int moveDir = new Vector2Int((int)Input.GetAxisRaw("Horizontal"), (int)Input.GetAxisRaw("Vertical"));
		// 	if (isUnitSelected)
		// 		selectedAnimateUnit.Move(moveDir, true);
		// }
	
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
						camMover.MoveTo(interimUnit, cameraSmoothTime);
				}

				switch (mode)
				{
					case Mode.Action:   //ACTION MODE
						if (isUnitHit)
							AddTargetTile(interimUnit);
						if (isTileHit)
							AddTargetTile(interimTile);
						break;

					case Mode.Move:     //MOVE MODE
						if (isTileHit && isTileHitEmpty)	//Restrict to empty tiles only
						{
							if (selectedAnimateUnit.Move(interimTile))	//Try Move unit
								camMover.MoveTo(interimTile, cameraSmoothTime);	//If move successful then focus camera
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
							targetTileStack.Pop();	//UNDO 1 TARGET TILE SELECT
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
	
	#region Sets
		//Public; can be called via unity events
		public void SelectUnit(AnimateUnit au)
		{
			//Clear tile highlights if a unit was already selected
			if (isUnitSelected) 
			{
				selectedAnimateUnit.ClearGhost();
				ClearSelectedUnitTileHighlights();
				selectedAbility = null;
			}

			//Set the selected unit
			selectedUnit = au;

			//Show move tile if unit is able to move
			ShowMoveTiles();

			onUnitChanged.Invoke(au);	//ie. Update UI, Play sounds,
		}

		public void SelectAbility(Ability a)	//aka. OnAbilityChanged()
		{
			//Checks
			if (!isUnitSelected)
			{
				Debug.LogWarning("No unit selected! Cannot select ability");
				return;
			}
			if (!selectedAnimateUnit.canAct)
			{
				Debug.LogWarning("Unit cannot perform any more abilities this turn");
				return;
			}

			//Set
			selectedAbility = a;

			//Recalculate target tiles
			selectedAnimateUnit.CalculateTargetTiles(selectedAbility);

			//Clear move tiles + Show target tiles + clear ghosts
			selectedAnimateUnit.ClearGhost();
			ClearAllTileHighlights();
			ShowTargetTiles();

			//Auto perform ability on self if required tiles set to 0
			if (selectedAbility.requiredTiles == 0)
				AddTargetTile(selectedAnimateUnit.currentTile);

			//Raise
			onAbilityChanged.Invoke(a);
		}

		/// <summary>
		/// Add a target tile to the casting stack and if the selected ability required target input is reached then perform the ability
		/// </summary>
		void AddTargetTile(Tile t)
		{
			if (selectedAbility.IsAcceptableTileType(selectedAnimateUnit, t))		//Check ability can accept this tile type
				if (selectedAnimateUnit.possibleTargetTiles.Contains(t))	//Check tile is within possible target tiles
					if (!targetTileStack.Contains(t))						//Can't select the same tile twice
						targetTileStack.Push(t);

			//Perform ability once required number of tiles reached
			if (targetTileStack.Count >= selectedAbility.requiredTiles)
			{
				SelectedUnitPerformAbility();
			}
		}
		void AddTargetTile(Unit u) => AddTargetTile(u.currentTile);		//Redirect because sometimes the raycast can only hit a unit

		//Enough tile targets chosen by user. Execute the selected ability
		void SelectedUnitPerformAbility()
		{
			//Perform
			selectedAnimateUnit.Act(selectedAbility, targetTileStack.ToArray());

			//Focus camera
			camMover.MoveTo(selectedAnimateUnit, cameraSmoothTime);

			//Clear target stack
			targetTileStack.Clear();

			//clear ability
			ClearSelectedAbility(selectedAnimateUnit.canMove);

			//Events
			onAbilityPerformed.Invoke(selectedAbility);
		}
	#endregion

	#region Tile Highlighting
		//Show a preview of target tiles 
		public void OnHoverPreview(Ability a)
		{
			//Has to be in Move mode
			if (mode != Mode.Move) return;

			//Has to be able to act
			if (!selectedAnimateUnit.canAct) return;

			selectedAnimateUnit.CalculateTargetTiles(a);
			selectedAnimateUnit.ClearGhost();
			ShowTargetTiles();
		}
		public void OnUnhoverPreview()
		{
			//Must be in move mode
			if (mode != Mode.Move) return;

			//Redraw
			ClearAllTileHighlights();
			ShowMoveTiles();
		}

		void ShowMoveTiles()
		{
			//NOTE: Active unit's MOVE highlights should be refreshed:
			// - At the start of each turn
			// - After another unit has summoned something
			if (!selectedAnimateUnit.canMove) return;

			if (selectedAnimateUnit.possibleMoveTiles.Length <= 0)
				selectedAnimateUnit.CalculateMoveTiles();

			//Highlight
			foreach (var t in selectedAnimateUnit?.possibleMoveTiles)
				t.SetColor(moveHighlight);
		}

		void ShowTargetTiles()
		{
			//NOTE: Active unit's ACTION highlights should be refreshed
			// - each time the selected ability is changed
			if (selectedAnimateUnit.possibleTargetTiles.Length <= 0) return;

			//Highlight
			foreach (var t in selectedAnimateUnit?.possibleTargetTiles)
				t.SetColor(actionHighlight);
		}
	#endregion

	#region Clears
		//Deselects the unit
		void ClearSelectedUnit()
		{
			if (!isUnitSelected) return;	//A unit should be selected

			onUnitCleared.Invoke();

			//Clear tile highlights and ghost
			ClearSelectedUnitTileHighlights();
			selectedAnimateUnit.ClearGhost();

			//Clear
			selectedUnit = null;
		}

        void ClearSelectedAbility(bool redrawMoveTiles = true)
		{
			if (!isUnitSelected) return;	//A unit should be selected

			onAbilityCleared.Invoke();

			//Clear
			selectedAbility = null;

			//Clear tile highlights
			if (isUnitSelected) 
				ClearSelectedUnitTileHighlights();

			//Redraw move highlights
			if (redrawMoveTiles) 
				ShowMoveTiles();
		}

		void ClearSelectedUnitTileHighlights()
		{
			if (!isUnitSelected) return;	//A unit should be selected

			//Clear move highlights
			if (selectedAnimateUnit.possibleMoveTiles != null)
				foreach (var t in selectedAnimateUnit.possibleMoveTiles)
					t.ClearColor();

			//Clear target highlights
			if (selectedAnimateUnit.possibleTargetTiles != null)
				foreach (var t in selectedAnimateUnit.possibleTargetTiles)
					t.ClearColor();
		}

		//Trying to avoid the accidental unhover glitch but still doesn't solve it
		void ClearAllTileHighlights()
        {
			foreach (var t in Map.current.tiles)
			{
				t.ClearColor();
			}
        }
		#endregion

	#region Assists
		//If T object hit then return true and output it
		bool TryGetRaycast<T>(out T hit) where T : MonoBehaviour
		{
			if (IsPointerOverGUIObject())	//Prevent click through
			{
				// Debug.Log("GUIObjectHit");
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

		bool IsPointerOverGUIObject()
		{
			//Set up the new Pointer Event
			var ped = new PointerEventData(EventSystem.current);

			//Set the Pointer Event Position to that of the mouse position
			ped.position = Input.mousePosition;

			//Raycast using the Graphics Raycaster and mouse click position
			GUIhits.Clear();
			gr.Raycast(ped, GUIhits);

			//For every result returned, output the name of the GameObject on the Canvas hit by the Ray
			// foreach (RaycastResult result in GUIhits)
			// 	Debug.Log("Hit " + result.gameObject.name);
			
			if (GUIhits.Count > 0)
				return true;
			return false;
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
	#endregion

	#region Debug
		void OnGUI()
		{
			if (!debug) return;
			GUILayout.Label("ActivityMode: " + mode);

			GUILayout.Label("is a unit hit?: " + isUnitHit);
			GUILayout.Label("is a tile hit?: " + isTileHit);

			GUILayout.Label("is a unit selected?: " + isUnitSelected);
			if (_selectedAbilityVar.value) GUILayout.Label("Selected Unit: " + _selectedUnitVar?.value?.name);

			GUILayout.Label("is an ability selected?: " + isAbilitySelected);
			if (_selectedAbilityVar.value) GUILayout.Label("Selected Ability: " + _selectedAbilityVar?.value?.name);

			GUILayout.Label("GUI hits count: " + GUIhits.Count);

			GUILayout.Label(string.Format("targetTileStack ({0}):", targetTileStack.Count));
			foreach (var t in targetTileStack)
				GUILayout.Label(t.name);
		}
	#endregion
	}
}