using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;
using pokoro.Patterns.Generic;
using StormRend.Units;
using System;
using StormRend.MapSystems.Tiles;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace StormRend.MapSystems
{
	[ExecuteInEditMode]
	public sealed class Map : Singleton<Map>    //Only one map per scene?
	{
		const float maxMapSize = 500f;

		//TODO
		//- Map editor needs to be able to place units and set them accordingly
		//- Map needs to hold a list of units for other things to be able to reference

		//Inspector
		[Header("Tiles")]
		[SerializeField, Range(1, 5), Tooltip("This map's tile XZ scale")] public float tileSize = 2;

		[Tooltip("Pallette of tile prefabs")]
		public Tile[] palette;
		[HideInInspector] public int selectedPrefabIDX = 0;

		//Properties
		public Tile selectedTilePrefab => palette?.Length == 0 ? null : palette?[selectedPrefabIDX];
		public bool isPaletteActive => palette != null && palette.Length > 0;

		//Members
		[HideInInspector] public List<Tile> tiles = new List<Tile>();
		static UnitRegistry ur;

#if UNITY_EDITOR
		[HideInInspector] public BoxCollider editorRaycastPlane;
#endif

	#region Core
		void Awake()
		{
			ur = UnitRegistry.current;
		}
		void OnEnable()
		{
#if UNITY_EDITOR
			// layerMask = 1 << gameObject.layer;
			// _root = this.transform;
			Selection.selectionChanged += OnSelected;
#endif
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

		/// <summary>
		/// Gets the nearest tile
		/// </summary>
		public bool TryGetNearestTile(Vector3 position, out Tile nearestTile)
		{
			nearestTile = null;
			float nearestSqrDist = Mathf.Infinity;
			//Loop through all tiles and find the nearest tile
			foreach (Tile t in tiles)
			{
				var sqrDist = Vector3.SqrMagnitude(t.transform.position - position);
				if (sqrDist < nearestSqrDist)
				{
					nearestSqrDist = sqrDist;
					nearestTile = t;
				}
			}
			return nearestTile ? true : false;
		}

		[ContextMenu("Delete All Tiles")]
		public void DeleteAllTiles()
		{
			while (transform.childCount > 0)
			{
				//Undo Works
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
			{
				t.DisconnectAll();
				EditorUtility.SetDirty(t);      //Actually saves the data
			}
		}
		public void GetTileTerrainCost(Tile tile) { }
	#endregion

	#region Pathfinding
		/// <summary>
		/// Calculates and returns a possible pathfinding solution
		/// </summary>
		/// <param name="map">The map</param>
		/// <param name="start">Starting tile</param>
		/// <param name="range">The range of movement</param>
		/// <param name="pathblockingUnitTypes">The type of units on tiles that blocks the path and will be filtered out</param>
		/// <returns></returns>
		public static Tile[] GetPossibleTiles(Map map, Tile start, int range, params Type[] pathblockingUnitTypes)
		{
			Debug.Assert(start, "Invalid Start tile");
			Debug.Assert(map, "Invalid map parameter");

			//TODO Check to make sure exclude
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
					//PASS if neighbour tile is unwalkable
					if (n is UnWalkableTile)
						continue;

					//PASS if neighbour tile has a unit on top that needs to be ignored
					if (UnitRegistry.AreUnitTypesOnTile(n, pathblockingUnitTypes))
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
						// n.parent = currentTile;
						if (n.G <= range) openList.Enqueue(n);
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
			return validMoves.ToArray();
		}
	#endregion
	}
}