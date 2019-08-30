using System.Collections.Generic;
using UnityEngine;

namespace StormRend
{
    namespace The.Great.Refactor
    {
        public class StateMachineImplementation
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

        /* -------- Organization
        --- Folder structure
        -- Conventions
        - No spaces in file/folder names

        [ScriptableObjects]
        Abilities
            Ally
            Enemy
        AI
            Delegates
            Variables

        [Scripts]
        --------
        |Editor|
            AbilityInspector
            EffectInspector
            EnumFlagsInspector
            ReadOnlyFieldInspector
            MapEditorInspector,Core,Scene
            DecorationPainterInspector,Core,Scene

        |Tests|
        -------
        AbilitySystem
            Ability.cs
            AttackManager.cs ?
            Effects
                Effect.cs
                Benefits / Curses / Defensive / Offsensive / Recovery / Runes
        AI
            Delegates
            Variables
        States
            GameStateDirector.cs
            AllyTurnState.cs
            EnemyTurnState.cs
            UI
                UIState.cs
                MainMenuState.cs
                PauseMenuState.cs
                SettingsMenuState.cs
                GameOverState.cs
                VictoryState.cs
        Commands
        Units
            Unit
            AllyUnit
            EnemyUnit
            SpiritCrystal
        UI
            AvatarFrame
            PulsingNode
            PulsingBar
            AbilityButton ?
            NextTurnButton  ?
            InfoPanel ?
        AnimatorStateMachineBehaviours
        FX
        CoreSystems
            Camera
            FileIO

            GameplayInteraction
                IInteraction.cs
                GameplayInteractionHandler.cs
            StateMachines
                State, StackState
                CoreStateMachine, TurnBasedStackStateMachine
            Mapping
                Connection
                Tile
                Map
                Pathfinder
            UndoSystem
                Undo
                ICommand
        Utility
            Attributes
                EnumFlagsAttribute.cs
                ReadOnlyFieldAttribute.cs
            Patterns
                Singleton<T>
                ScriptableSingleton<T>
        |Defunct|
            "Put old shit here"
            TrashScripts
        */

        //Renames, Cleanups
        // - AbilityEditorUtility > SREditorUtility


        //Unit Testing

        //Conventions
        public class Conventions
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
    }
}