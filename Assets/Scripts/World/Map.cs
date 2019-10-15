using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using pokoro.Patterns.Generic;
using StormRend.Units;
using StormRend.Variables;

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
		public bool isPaletteActive => palette != null && palette.Length != 0;
		public List<Unit> mapUnits => mapUnits;

		//Members
		[HideInInspector] public List<Tile> tiles = new List<Tile>();
		List<Unit> _mapUnits;

#if UNITY_EDITOR
		[HideInInspector] public BoxCollider editorRaycastPlane;
#endif

		#region Core
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

		public static Tile[] AStar(Map map, Tile start, Tile end)
		{
			return new Tile[0];
		}
	}
}