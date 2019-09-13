using System.Collections.Generic;
using UnityEngine;
using BhaVE.Patterns;
using System.Linq;
using StormRend.Utility.Attributes;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace StormRend.Systems.Mapping
{
	[ExecuteInEditMode]
	public sealed class Map : Singleton<Map>
	{
		const float maxMapSize = 500f;

		#region Inspector
		[SerializeField] [Range(1, 5)] [Tooltip("This map's tile XZ scale")] internal float tileSize = 2;
		[HideInInspector] [SerializeField] internal List<Tile> tiles = new List<Tile>();
		[SerializeField] internal GameObject[] palette;
		#endregion

		internal int selectedPrefabIDX;
		internal GameObject selectedTilePrefab => palette?.Length == 0 ? null : palette?[selectedPrefabIDX];
		internal bool isPaletteActive => palette != null && palette.Length != 0;

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
		void OnDisable()
		{
#if UNITY_EDITOR
			Selection.selectionChanged -= OnSelected;
#endif
		}

		void OnValidate()
		{
			//Make sure any prefabs injected are actually tiles
			if (isPaletteActive)
			{
				foreach (var t in palette)
				{
					if (!t.GetComponentInChildren<Tile>())
					{
						Debug.LogWarningFormat("{0} is not a Tile! Removing...", t.GetType().Name);
						palette = palette?.Where(x => x != t).ToArray();
					}
				}
			}
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