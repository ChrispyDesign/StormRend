using System.Collections.Generic;
using pokoro.BhaVE.Core.Events;
using StormRend;
using UnityEditor;
using UnityEngine;

namespace The.Great.Refactor.Brainstorm
{
    /*
	>> Brainstorm
	- Glory and Blizzard are essentially just integer numbers
		Implementation Ideas:
		- Use advanced SO variables with inbuilt SO events that trigger when changed
		- global Scriptable object that holds all the crucial data
	- Selected Unit
		- SO variable
	- Current active units
		- Just find all units at start of scene

    >> Custom StormRend Variables
    UnitListVar : BhaveVar<List<Unit>>
    UnitVar : BhaveVar<Unit>

    >> Game Data
    [ActiveUnits] : UnitListVar
        - Players
        - Enemies
    [DeadUnits]? : UnitListVar
    [EnemyTargetList] : UnitListVar
        Working unit target list for enemies. Updated and controller by the enemy's AI system
    [SelectedUnit] : UnitVar
        The currently selected unit
    [Glory] : BhaveInt
        Holds the current glory level
    [Blizzard] : BhaveInt
        Holds the current blizzard level

	>> Main Elements >>
	[CoreSystems]
		[V] UltraStateMachine: Turn based stackable state machine
		[V] GameDirector: Extra component to control the statemachine and other minor game logic
		[ ] BlizzardApplicator: Applies the blizzard to selected units
		[ ] UserInputHandler (PlayerController)
		[ ] UndoSystem

	[Game Variables and Events]
		[V] SO Variables (BhaveVar<T>)
		[V] SO Events (BhaveEvent)
		[V] IntVariableHandler: Listens for events from variables and limits it's value
		[X] VariableEventListener: Listens to variable changes and invokes local UnityEvent
		[X] VariableLimiter: Puts limits on variables (need internal onChanged event?)

	[Glory, Blizzard]
		IntVariableHandler
        • Limit variable min/max
        • Events
            -> Meter (UI)
                - Increase(), Decrease()
            -> BlizzardApplicator : MonoBehaviour
                - Refs: ActiveUnits, EnemyTargetList
            -> Play sounds, other

	[Camera and Event]
		[ ] Use unity inbuilt ISelectable/hoverable/etc
		[ ] Stretch goal: Use cinemachine

	[EventSystem]
		[ ] Replace current selectable/hoverable system with Unity's inbuilt event system
        - Unit class
		- Use PhysicsRaycaster instead of CameraRaycaster

	[Map System]
		[V 90%] Tile: A tile holds a list of other connected tiles and its traversal costs
		[V 90%] Map: A map is mostly a list of tiles
		[V 90%] MapEditor: Rapidly create, edit and connect tiles
		[V 75%] PropPainter: Prefab painting tool for use with map
		[ ] Pathfinder (Proposal): Maybe should be called MapExtentions? Pathfinding functionality

	[UI]
		[ ] Meter (for Glory and Blizzard meters)
		[ ] AvatarSelectButton
		[ ] AbilitySelectButton
		[ ] InfoPanel
		[V] Panels: (Includes states)
			MainMenu
			Gameplay
			Settings
			Pause
			Win/Lose?

	[Ability System]
		Refactor goals:
		- Get rid of getters and setters
		- Simplify and expose only essential APIs ie. Ability.Perform()
		- Decouple from UI stuff
		- Work with new improved map system
		- AbilityType : Enum ie. Passive, First, Second, Third, etc
		Important members:
		- ability name, Not Required! Just use Object.name
		- animNumber? Maybe this could hold the actual animation
		- Eliminate RowData; Use Array[7][7] instead
		- TargetableTiles > Convert to bitmask
		Important APIs to expose:
		- 

	[Units]
		Refactor goals:
		- Get rid of getters and setters
		- Simplify and expose only essential APIs ie. Ability.Perform()
		- Decouple from UI stuff
		- Work with new improved map system
		- Use unity inbuilt event system
		- Duplicate mesh > ghost mesh
		- Just hold a single list of abilities instead of multiple lists; sort by AbilityType
		- Get rid of current UnityEvents
		Important APIs to expose:
		- MoveTo(Tile tile), MoveTo(Vector2Int direction)?
		- ProposeMove(Tile tile); displays the ghost mesh
		- TakeDamage(int damage); current one is OK (Because I wrote it)
		- Die(); OK

	[BhaVE (AI): Awesome Behaviour Tree Editor!]
		[V] BhaVE segregated into Runtime and Editor Dlls
		[V] Project is able to build without errors

	[Sequencing]
		[V 85%] Dialog/subtitle system - OK
		[ ] Cutscenes; Need to learn more about timeline and cinemachine
	*/

    public class StateMachineImplementation : Completed
    {
        public class State : ScriptableObject
        {
            public void OnEnter() { }
            public void OnUpdate() { }
            public void OnExit() { }
        }
        public class StackState : State
        {
            public void OnCover() { }
            public void OnUncover() { }
        }
        // public class TurnState : State
        // {
        //     public void OnNext() { }
        // }
        public class CoreStateMachine
        {
            public virtual State currentState { get; protected set; }

            //--------------------------------
            public void Switch(State state)
            {
                currentState?.OnExit();
                currentState = state;
            }
            protected virtual void Update()
            {

            }
        }
        public class TurnBasedStackStateMachine : CoreStateMachine
        {
            protected Stack<State> states = new Stack<State>();

            public new State currentState { get; }

            //-----------------------------------------------
            public void Push(StackState stackState) { }
            public StackState Pop()
            {
                return new StackState();
            }
            //-----------------
            public void Insert(State state) { }
            public void Remove(State state) { }
            public void NextTurn() { }
            public void PrevTurn() { }   //?
                                         //-----------------
        }
    }

    public class MapImplementation : Completed
    {
        /// <uber>
        /// • Medical
        /// • Sort out and determine steps to finish progress
        /// • Uber sticker
        /// </uber>
        ///
        /// <brainstorm>
        /// • Tiles shouldn't need to know if a Unit is on top or not;
        /// • The unit should contain data about which tile it's on. That way we can just iterate through all the units instead of all the tiles
        /// • Map will hold it's tiles xyz scales ratios
        ///
        /// Q. How do units move?
        /// A. Units to move using Unit|Path.SetDestination(Tile|Vector3)
        ///
        /// Q. How can the Valkyrie push enemies off the level?
        /// A. There will be void tiles that are invisible.
        /// These should be placed where the designer deem a unit can fall to its death in the level.
        /// The light fall algorithm will simply push (move) the victim unit as usual but using SetDestination()
        ///
        /// Q. How are tiles connected?
        /// A. A method to automatically connect neighbour tiles by distance or radius will be available in Map.
        /// Maybe another method to connect the closest adjacent "manhatten" neighbour tiles.
        ///
        /// Q. Ho
        /// </brainstorm>
    }

    public class Completed { }




    //Renames, Cleanups
    // - AbilityEditorUtility > SREditorUtility
    // - PlayerController > UserInputHandler

    //Unit Testing

    #region Conventions
    internal class Conventions
    {
        //Fields/Symbols
        [SerializeField] float privateShownOnInspector;
        [HideInInspector] [SerializeField] float PrivateNotShownOnInspectorButSerialized;
        public float avoidMe;     //Free variable that can be modified by anything and anyone

        //Properties
        //Shown on inspector, but read only in the assembly/codebase
        [SerializeField] float _propertyBackingField;
        public float propertyBackingField
        {
            get => _propertyBackingField;

        }

        void Something()
        {
            Debug.Log("somethign");
        }

        void UseExpressionBodyMethodsForCleanerCode() => Debug.Log("This is clean!");


        //Privates
        bool isPrivate = true;      //Implicit private

        /*
		Big classes
		- Classes over 200-300 lines of code should be split up using partial

		Try catch blocks
		try
		{
			if (blah)
			else if (bleh)
			else
				throw new InvalidOperationException("This is illegal!");
		}
		catch (Exception e)
		{
			Debug.LogWarning(e);
		}
	*/
    }
    #endregion
}