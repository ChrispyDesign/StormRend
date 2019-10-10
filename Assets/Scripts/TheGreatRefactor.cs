using System.Collections.Generic;
using UnityEditor;
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

		public class MapImplementation
		{
			/// <uber>
			/// • Medical
			/// • Sort out and determine steps to finish progress
			/// • Uber sticker
			/// </uber>
			///
			/// <brainstorm>
			/// • Tiles shouldn't need to know if a Unit is on top or not.
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

			class Connection
			{
				Tile target;
				float cost;
			}

			class Tile : MonoBehaviour
			{
				List<Connection> connections = new List<Connection>();
			}

			class Map : MonoBehaviour   //Map.cs
			{
				List<Tile> tiles = new List<Tile>();
			}

			[ExecuteInEditMode]
			class Decorator : MonoBehaviour
			{
				public enum CollisionTest { RendererBounds, ColliderBounds }
				public LayerMask layerMask;
				public Transform rootTransform;
				public float brushRadius = 0.5f;
				public float brushDensity = 0.25f;
				[Range(0, 360)] public float maxRandomRotation = 360f;
				[Range(0, 360)] public float rotationStep = 90f;
				public CollisionTest collisionTest;
				[Range(0, 1)] public float maxIntersectionVolume = 0; //?
                [HideInInspector] public bool randomizeAfterStamp = true;
                [HideInInspector] public bool followOnSurface = true;
                [HideInInspector] public int selectedPrefabIndex = 0;
                public GameObject[] prefabPalette;

                public GameObject selectedPrefab => prefabPalette?[selectedPrefabIndex];


                [ContextMenu("Delete Children")]
                void DeleteChildren()
                {
                    while (transform.childCount > 0)
                        DestroyImmediate(transform.GetChild(0).gameObject);
                }
			}

            [CustomEditor(typeof(Decorator))]
			partial class DecoratorEditor : Editor     //MapBuilderEditor.cs
			{
                Texture2D[] paletteImages;
                GameObject stamp;
                List<GameObject> erase = new List<GameObject>();
                Vector3 worldCursor;
                Decorator mb;
                List<Bounds> overlaps = new List<Bounds>();
                List<GameObject> overlappedGameObjects = new List<GameObject>();

                //Init the stamp, get the target
                void OnEnable() {}
                //Cleanup stamp etc
                void OnDisable() {}
                //Create a new stamp according to settings
                void CreateNewStamp() {}
                void PerformErase() {}
                void PerformStamp() {}
                void RotateStamp() {}
                void AdjustMaxScale(float s) {}
			}

			partial class DecoratorEditor : Editor     //MapBuilderEditor.Inspector.cs
			{
                static void CreateDecorator() {}
                void RefreshPaletteImages(Decorator d) {}
                public override void OnInspectorGUI() {}
			}

			partial class DecoratorEditor : Editor     //MapBuilderEditor.SceneView.cs
			{
                void OnSceneGUI() {}
                void OverlapCapsule(Vector3 top, Vector3 bottom, float brushRadius, LayerMask layerMask) {}
                void HandleKey(KeyCode keyCode) {}
                void DrawStamp(Vector3 center, Vector3 normal) {}
                Bounds Intersection(Bounds A, Bounds B) { return new Bounds(); }
                void DrawEraser(Vector3 center, Vector3 normal) {}
                public override bool RequiresConstantRepaint() { return true; }
			}

			static class Pathfinder
			{
				static Tile[] FindValidMoves(Map map, Unit unit) { return new Tile[0]; }
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
            GameStateMachine.cs
            Unit
                AllyTurnState.cs
                EnemyTurnState.cs
                CrystalTurnState.cs
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
		// - PlayerController > UserInputHandler

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