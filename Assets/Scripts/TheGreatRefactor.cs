﻿using System.Collections.Generic;
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

    >> Custom Game Variables
    UnitListVar : BhaveVar<List<Unit>>
    UnitVar : BhaveVar<Unit>

    >> Game Data
    - [ActiveUnits] : UnitListVar
        - Players
        - Enemies
    - [DeadUnits]? : UnitListVar
    - [EnemyTargetList] : UnitListVar
        Working unit target list for enemies. Updated and controller by the enemie's AI system
    - [SelectedUnit] : UnitVar
        The currently selected unit
    - [Glory] : BhaveInt
        Holds the current glory level
    - [Blizzard] : BhaveInt
        Holds the current blizzard level

	>> Main Elements
	[CoreSystems]
		[Completed] UltraStateMachine: Turn based stackable state machine
		[Completed] GameDirector: Extra component to control the statemachine and other
		[Refactor] UserInputHandler (PlayerController)
		[Refactor] UndoSystem

	[Game Variables and Events]
		[Completed] SO Variables (BhaveVar<T>)
		[Completed] SO Events (BhaveEvent)

	[Glory]
        VariableListener
        • Limit variable min/max
        • Events
            -> GloryMeter : Meter (UI)
                - Increase(), Decrease()
            -> Play sounds, other

	[Blizzard]
        VariableListener
        • Events
            -> BlizzardMeter : Meter (UI)
            -> BlizzardApplicator : MonoBehaviour
                - Refs: ActiveUnits, EnemyTargetList
            -> Play sounds, other

	[Camera and Event]
		Use unity inbuilt ISelectable/hoverable/etc
		Stretch goal: Use cinemachine

	[EventSystem]
		Replace current selectable/hoverable system with Unity's inbuilt event system
        - Unit class
		- Use PhysicsRaycaster instead of CameraRaycaster

	[Map System]
		O Tile: A tile holds a list of other connected tiles and its traversal costs
		O Map: A map is mostly a list of tiles
		O MapEditor: Rapidly create, edit and connect tiles
		- PropPainter: Prefab painting tool for use with map
		- Pathfinder (Proposal): Maybe should be called MapExtentions? Pathfinding functionality

	[UI]
		- Meter (for Glory and Blizzard meters)
		- AvatarSelectButton
		- AbilitySelectButton
		- InfoPanel
		O Panels: (Includes states)
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

	[BhaVE (AI): Awesome Behaviour Tree Editor Completed]
		O Editor and Runtime Segregated DLL

	[Sequencing]
		O Dialog/subtitle system - OK
		- Cutscenes; Need to learn more about timeline and cinemachine
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