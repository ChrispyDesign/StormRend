using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace StormRend.Systems.Mapping
{
	/// Brainstorm
	/// ? Should this class be the base for it's map editor/builder?
	/// ! It very much can be. it will save creating another class. All tiles would be parented to this.
	[ExecuteInEditMode]
	public sealed class Map : MonoBehaviour
	{
		//Constants
		const float maxMapSize = 400f;

		#region Inspector
		[SerializeField] Transform _root;
		internal Transform root
		{
			get
			{
				if (!_root) return root;
				else return _root;
			}
			set => _root = value;
		}
		[SerializeField] [Range(0.1f, 10)] [Tooltip("This map's tile XZ scale")] internal float tileSize = 2f;
		[SerializeField] List<GameObject> tilePrefabs = new List<GameObject>();
		internal Tile selectedTilePrefab;

		[HideInInspector] [SerializeField] internal List<Tile> tiles = new List<Tile>();

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

		void OnSelected()
		{
			//Create a new raycast plane if it doesn't exist
			if (!editorRaycastPlane) CreateEditorRaycastPlane();
		}
		#endregion

		void CreateEditorRaycastPlane()
		{
			//Create an extremely large plane colider that is used only for editor raycasting
			editorRaycastPlane = gameObject.AddComponent<BoxCollider>();
			editorRaycastPlane.center = transform.position;         //Position
			editorRaycastPlane.size = new Vector3(maxMapSize, 0, maxMapSize);   //Size
			editorRaycastPlane.isTrigger = true;
			editorRaycastPlane.hideFlags = HideFlags.HideAndDontSave | HideFlags.HideInInspector;   //Hide
		}
		//Maybe these should Editor methods
		public void ConnectNeighbourTilesByDistance(float connectRadius)
		{
		}

		public void ConnectTilesByManhattan()
		{
		}

		public void UpdateTiles()
		{
		}

		public void GetTileTerrainCost(Tile tile) { }

		public static Tile[] AStar(Map map, Tile start, Tile end)
		{
			return new Tile[0];
		}

	}
}


// public enum BoundsType
// {
// 	RendererBounds,
// 	ColliderBounds
// }
// public BoundsType boundsType;
// [SerializeField] Color hoverTint = Color.yellow;
// [SerializeField] Color attackTint = Color.red;
// [SerializeField] Color moveTint = Color.blue;