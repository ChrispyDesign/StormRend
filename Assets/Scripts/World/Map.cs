using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using pokoro.Patterns.Generic;
using StormRend.Units;
using StormRend.Variables;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace StormRend.Systems.Mapping
{
	[ExecuteInEditMode]
	public sealed class Map : MonoBehaviour //Singleton<Map>	//Only one map per scene?
	{
		const float maxMapSize = 500f;

		//TODO
		//- Map editor needs to be able to place units and set them accordingly
		//- Map needs to hold a list of units for other things to be able to reference

		//TEMP WORKFLOW: Edit desired tile highlights settings in here, and on start the settings will get transferred over to Tile.tileHighlights
		public List<TileHighlightColor> tileHighlightsSettings = new List<TileHighlightColor>();

		//Inspector
		[SerializeField, Range(1, 5), Tooltip("This map's tile XZ scale")] public float tileSize = 2;

		[Tooltip("Pallette of tile prefabs")]
		public Tile[] palette;
		public int selectedPrefabIDX = 0;

		//Properties
		public Tile selectedTilePrefab => palette?.Length == 0 ? null : palette?[selectedPrefabIDX];
		public bool isPaletteActive => palette != null && palette.Length > 0;

		//Members
		[HideInInspector] public List<Tile> tiles = new List<Tile>();
		UnitRegistry ur;

		// public List<Unit> mapUnits => _mapUnits;
		// List<Unit> _mapUnits;	//Is this required? What should actually set this? Should we just use UnitRegistry

#if UNITY_EDITOR
		[HideInInspector] public BoxCollider editorRaycastPlane;
#endif

		#region Core
		void Awake()
		{
			ur = UnitRegistry.singleton;
		}
		void OnEnable()
		{
#if UNITY_EDITOR
			// layerMask = 1 << gameObject.layer;
			// _root = this.transform;
			Selection.selectionChanged += OnSelected;
#endif
		}
		void Start()
		{
			//Transfer semi-global highlight colours over to static tile highlight colours
			Tile.highlightColors = tileHighlightsSettings;
		}
		void OnDisable()
		{
#if UNITY_EDITOR
			Selection.selectionChanged -= OnSelected;
#endif
		}

#if UNITY_EDITOR
		void OnSelected()
		{
			//Create a new raycast plane if it doesn't exist
			if (!editorRaycastPlane) CreateEditorRaycastPlane(maxMapSize);
		}
#endif
		void Update()
		{
			LockRotationAndScale();

			void LockRotationAndScale()
			{
				transform.rotation = Quaternion.identity;
				transform.localScale = Vector3.one;
			}
		}
		#endregion

#if UNITY_EDITOR
	#region Assists
		void CreateEditorRaycastPlane(float mapSize)
		{
			//Create an extremely large plane colider that is used only for editor raycasting
			editorRaycastPlane = gameObject.AddComponent<BoxCollider>();
			editorRaycastPlane.center = transform.position;                 //Position
			editorRaycastPlane.size = new Vector3(mapSize, 0, mapSize);     //Size
			editorRaycastPlane.isTrigger = true;
			editorRaycastPlane.hideFlags = HideFlags.HideAndDontSave | HideFlags.HideInInspector;   //Hide
		}
		[ContextMenu("Delete All Tiles")]
		public void DeleteAllTiles()
		{
			while (transform.childCount > 0)
			{
				Undo.DestroyObjectImmediate(transform.GetChild(0).gameObject);
			}
			tiles.Clear();
		}
	#endregion
#endif

	#region Connections
		/// <summary>
		/// Clear all tile connections
		/// </summary>
		public void ClearAllTileConnections()
		{
			foreach (var t in tiles)
				t.DisconnectAll();
		}
		public void GetTileTerrainCost(Tile tile) { }
	#endregion

	#region Pathfinding
		public static List<Tile> GetValidMoves(Map map, Tile start, int range, params Type[] unitTypeToIgnore)
		{
			List<Tile> validMoves = new List<Tile>();
			Queue<Tile> openList = new Queue<Tile>();
			List<Tile> closedList = new List<Tile>();

			//Add starting point to open list
			openList.Enqueue(start);

			while (openList.Count > 0)
			{
				//Init current tile to openlist's first node and remove it from the openlist
				//Add it to the closed list cuz it's being searched
				var currentTile = openList.Dequeue();

				if (!closedList.Contains(currentTile))
					closedList.Add(currentTile);

				List<Tile> neighbours = currentTile.connections.ToList();

				//Search through the neighbours until we find the best travel cost to another tile
				foreach (var n in neighbours)
				{
					//Pass if neighbour tile is unwalkable
					if (n is UnWalkableTile) continue;

					//Pass if neighbour tile has a unit that needs to be ignored
					foreach (var u in map.ur.aliveUnits)
						//If unit is a type that needs to be ignored...
						if (unitTypeToIgnore.Contains(u.GetType()))
							//If unit is on this neighbour tile...
							if (u.currentTile == n)
								continue;

					//connected tile checked
					if (!closedList.Contains(n))
						closedList.Add(n);

					//Set some costs???
					var newMovementCostToNeighbour = currentTile.G + 1;
					if (newMovementCostToNeighbour < n.G || !openList.Contains(n))
					{
						n.G = newMovementCostToNeighbour;
						n.H = 1;
						n.parent = currentTile;

						if (n.G <= range)
							openList.Enqueue(n);
					}
				}

				//Can this be optimized?
				if (currentTile.G > 0 && currentTile.G <= range && !validMoves.Contains(currentTile))
					validMoves.Add(currentTile);
			}

			foreach (var t in closedList)
			{
				t.G = 0;
				t.H = 0;
			}

			return validMoves;
		}
	#endregion
	}
}