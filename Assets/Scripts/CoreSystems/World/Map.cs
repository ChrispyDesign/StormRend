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
		public enum BoundsType
		    { RendererBounds, ColliderBounds }
        //Constants
        const float maxMapSize = 400f;

    #region Inspector
		[SerializeField] [Range(1, 5)] [Tooltip("This map's tile XZ scale")] internal float tileSize = 2;
		[SerializeField] internal BoundsType boundsType;
        [SerializeField] internal GameObject[] palette;
        internal int selectedPrefabIDX;

        internal GameObject selectedTilePrefab => palette?.Length == 0 ? null : palette?[selectedPrefabIDX];
        internal bool isPaletteActive => palette != null && palette.Length != 0;

        [HideInInspector][SerializeField] internal List<Tile> tiles = new List<Tile>();

#if UNITY_EDITOR
        [HideInInspector] public BoxCollider editorRaycastPlane;
#endif
        #endregion

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



// [SerializeField] Color hoverTint = Color.yellow;
// [SerializeField] Color attackTint = Color.red;
// [SerializeField] Color moveTint = Color.blue;'


// [Tooltip("Where this editor will parent the painted tiles. Defaults to this Map object")]
// [SerializeField] Transform _root;
// internal Transform root
// {
//     get
//     {
//         if (!_root)
//         {
//             _root = transform;
//             return _root;
//         }
//         else
//             return _root;
//     }
//     set => _root = value;
// }
// [SerializeField] internal LayerMask layerMask;