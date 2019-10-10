using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using pokoro.Patterns.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace StormRend.Systems.Mapping
{
	[ExecuteInEditMode]
    public sealed class Map : Singleton<Map>
    {
		public enum BoundsType { RendererBounds, ColliderBounds }

        const float maxMapSize = 400f;

    #region Inspector
		[SerializeField] internal BoundsType boundsType;
		[Range(1, 5)] [Tooltip("This map's tile XZ scale")] public float tileSize = 2;
        public GameObject[] palette;
        public int selectedPrefabIDX;

        public GameObject selectedTilePrefab => palette?.Length == 0 ? null : palette?[selectedPrefabIDX];
        public bool isPaletteActive => palette != null && palette.Length != 0;

        [HideInInspector] public List<Tile> tiles = new List<Tile>();

#if UNITY_EDITOR
        [HideInInspector] public BoxCollider editorRaycastPlane;
#endif
        #endregion

        #region Core
        void OnEnable()
        {
#if UNITY_EDITOR
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
        #endregion

#if UNITY_EDITOR
        void CreateEditorRaycastPlane(float mapSize)
        {
            //Create an extremely large plane colider that is used only for editor raycasting
            editorRaycastPlane = gameObject.AddComponent<BoxCollider>();
            editorRaycastPlane.center = transform.position;                 //Position
            editorRaycastPlane.size = new Vector3(mapSize, 0, mapSize);     //Size
            editorRaycastPlane.isTrigger = true;
            editorRaycastPlane.hideFlags = HideFlags.HideAndDontSave | HideFlags.HideInInspector;   //Hide
        }
#endif
        [ContextMenu("Delete All Tiles")]
        void DeleteAllTiles()
        {
            while (transform.childCount > 0)
                DestroyImmediate(transform.GetChild(0).gameObject);

            tiles.Clear();
        }

        //Maybe these should Editor methods
        public void ConnectNeighbourTilesByDistance(float connectRadius) {}
        public void ConnectNeighbourTilesByManhattan() {}
        public void GetTileTerrainCost(Tile tile) {}

        public static Tile[] AStar(Map map, Tile start, Tile end)
        {
            return new Tile[0];
        }
    }
}