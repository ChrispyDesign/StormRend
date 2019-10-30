using System.Collections.Generic;
using pokoro.Patterns.Generic;
using StormRend.Abilities;
using StormRend.CameraSystem;
using StormRend.MapSystems;
using StormRend.MapSystems.Tiles;
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
		public bool leftClickUp;
		public bool rightClicked;
		public bool rightClickUp;

		public void Refresh()
		{
			leftClicked = Input.GetMouseButtonDown(lmb);
			leftClickUp = Input.GetMouseButtonUp(lmb);
			rightClicked = Input.GetMouseButtonDown(rmb);
			rightClickUp = Input.GetMouseButtonUp(rmb);
		}
	}

	public class UserInputHandler : Singleton<UserInputHandler>
	{
		//Enums
		public enum ActivityMode
		{
			Move,
			Action,      //Cast spells, attack, summon, etc
			Idle
		}

		//Inspector
		[SerializeField] LayerMask raycastLayerMask;

		[Tooltip("A reference to the State object that is considered to be the player's state ie. AllyState")]
		[Space(10), SerializeField] State playersState = null;

		[Space(10), SerializeField] UnitVar _selectedUnit = null;
		[ReadOnlyField, SerializeField] Ability _selectedAbility = null;

		[Header("Tile Colors")]
		[SerializeField] TileHighlightColor moveHighlight = null;
		[SerializeField] TileHighlightColor actionHighlight = null;

		[Header("Camera")]
		[SerializeField] float cameraSmoothTime = 1.75f;

		//Properties
		ActivityMode mode
		{
			get
			{
				//If an ability is current selected then unit can perform ACTION
				if (isAbilitySelected && selectedAnimateUnit.canAct)
					return ActivityMode.Action;
				//If only unit selected and can move the unit can perform MOVE
				else if (isUnitSelected && selectedAnimateUnit.canMove)
					return ActivityMode.Move;
				//Unit not selected
				else
					return ActivityMode.Idle;
			}
		}
		public Unit selectedUnit
		{
			get => _selectedUnit.value;
			internal set => _selectedUnit.value = value;
		}
		public AnimateUnit selectedAnimateUnit => (selectedUnit != null) ? selectedUnit as AnimateUnit : null;
		public bool isUnitSelected => selectedUnit != null;
		//Ability selection
		public Ability selectedAbility
		{
			get => _selectedAbility;
			internal set => _selectedAbility = value;
		}
		public bool isAbilitySelected => selectedAbility != null;
		public bool isPlayersTurn { get; set; } = true;		//If the current game state matches the player's state?
		bool notEnoughTargetTilesSelected => targetTileStack.Count < selectedAbility.requiredTiles;

		//Events
		[Space(5)]
		public UnitEvent OnUnitChanged;
		public UnityEvent OnUnitCleared;
		public AbilityEvent OnAbilityChanged;
		public AbilityEvent OnAbilityPerformed;
		public UnityEvent OnAbilityCleared;

		//Members
		FrameEventData e;   //The events that happenned this frame
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

	#region Core
		void Start()
		{
			//Inits
			cam = MasterCamera.current.camera;
			camMover = cam.GetComponent<CameraMover>();
			gr = FindObjectOfType<GraphicRaycaster>();	//On the one and only canvas
			_selectedUnit.value = null;
			_selectedAbility = null;

			//Asserts
			Debug.Assert(cam, "Camera could not be located!");
			Debug.Assert(camMover, "CameraMover could not be located!");
			Debug.Assert(_selectedUnit, "No Selected Unit SOV!");
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

	#endregion
		//--------------------- PROCESS EVENTS -----------------------------
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
					if (mode != ActivityMode.Action)
						camMover.MoveTo(interimUnit, cameraSmoothTime);
				}

				//PLAYER'S TURN
				if (isPlayersTurn)
				{
					switch (mode)
					{
						case ActivityMode.Action:   //ACTION MODE
							if (isUnitHit)
								AddTargetTile(interimUnit);
							if (isTileHit)
								AddTargetTile(interimTile);
							break;
						case ActivityMode.Move:     //MOVE MODE
							if (isTileHit && isTileHitEmpty)	//Restrict to empty tiles only
							{
								if (selectedAnimateUnit.Move(interimTile))	//Move unit
									camMover.MoveTo(interimTile, cameraSmoothTime);	//If move successful then focus camera
							}
							goto case ActivityMode.Idle;	//Fall through
						case ActivityMode.Idle:     //IDLE MODE
							if (isUnitHit && interimUnit is AnimateUnit)
								SelectUnit(interimUnit as AnimateUnit);
							break;
					}
				}

			}
			else if (e.rightClickUp)	//RIGHT CLICK RELEASED
			{
				if (isPlayersTurn)
				{
					switch (mode)
					{
						case ActivityMode.Action:	//ACTION MODE
							if (notEnoughTargetTilesSelected && targetTileStack.Count > 0)
								targetTileStack.Pop();	//UNDO 1 TARGET TILE SELECT
							else
								ClearSelectedAbility();	//CLEAR ABILITY
							break;
						case ActivityMode.Move:		//MOVE MODE
							ClearSelectedUnit();		//CLEAR UNIT
							break;
					}
				}
			}
			else 	//HOVER
			{
				switch (mode)
				{
					case ActivityMode.Move:		//MOVE
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
					// case ActivityMode.Idle:
					// 	isTileHit = TryGetRaycast<Tile>(out interimTile); //!!! MAKE SURE THE RAYCAST LAYERS ARE CORRECTLY SET !!!
					// 	if (isTileHit)
					// 	break;
				}
			}
		}
		//----------------------------------------------------------------
		void OnGUI()
		{
			if (!debug) return;
			GUILayout.Label("ActivityMode: " + mode);

			GUILayout.Label("is a unit hit?: " + isUnitHit);
			GUILayout.Label("is a tile hit?: " + isTileHit);

			GUILayout.Label("is a unit selected?: " + isUnitSelected);
			GUILayout.Label("Selected Unit: " + _selectedUnit?.value?.name);

			GUILayout.Label("is an ability selected?: " + isAbilitySelected);
			GUILayout.Label("Selected Ability: " + _selectedAbility?.name);

			GUILayout.Label("GUI hits count: " + GUIhits.Count);

			GUILayout.Label(string.Format("targetTileStack ({0}):", targetTileStack.Count));
			foreach (var t in targetTileStack)
				GUILayout.Label(t.name);
		}

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
			if (au.canMove)	ShowMoveTiles();

			OnUnitChanged.Invoke(au);	//ie. Update UI, Play sounds,
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
				Debug.LogWarning("Unit has moved and acted. Cannot select any more abilities this turn");
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

			//Raise
			OnAbilityChanged.Invoke(a);
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
			OnAbilityPerformed.Invoke(selectedAbility);
		}
	#endregion

	#region Tile Highlighting
		//Show a preview of target tiles 
		public void OnPointerEnterPreview(Ability a)
		{
			//Has to be in Move mode
			if (mode != ActivityMode.Move) return;

			selectedAnimateUnit.CalculateTargetTiles(a);
			selectedAnimateUnit.ClearGhost();
			ShowTargetTiles();
			// Debug.LogFormat("OnPreviewTargetHighlight({0})", a.name);
		}
		public void OnPointerExitPreview()
		{
			//Must be in move mode
			if (mode != ActivityMode.Move) return;

			//Redraw
			ClearAllTileHighlights();
			ShowMoveTiles();
		}

		void ShowMoveTiles()
		{
			//NOTE: Active unit's MOVE highlights should be refreshed:
			// - At the start of each turn
			// - After another unit has summoned something
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

			OnUnitCleared.Invoke();

			//Clear tile highlights and ghost
			ClearSelectedUnitTileHighlights();
			selectedAnimateUnit.ClearGhost();

			//Clear
			selectedUnit = null;
		}

        void ClearSelectedAbility(bool redrawMoveTiles = true)
		{
			if (!isUnitSelected) return;	//A unit should be selected

			OnAbilityCleared.Invoke();

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
	#endregion
	}
}