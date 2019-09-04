using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StormRend.Systems.Mapping
{
	/// Brainstorm
	/// ? Should this class be the base for it's map editor/builder?
	/// ! It very much can be. it will save creating another class. All tiles would be parented to this.
	[ExecuteInEditMode]
	public sealed class Map : MonoBehaviour
	{
		const float maxMapSize = 5000f;

		public Transform root;
		public LayerMask layerMask = ~0;
		// [SerializeField] internal Vector3 tileScale = new Vector3(2, 1, 2);  //XZscale of 2
		[SerializeField][Range(0.1f, 10)] internal float tileScaleXZ = 2f;

		//Palette
		[SerializeField] List<GameObject> tilePrefabs = new List<GameObject>();
		public bool selectedTilePrefab;



		[HideInInspector] [SerializeField] List<Tile> tiles;

		//Test editor raycasting
		[HideInInspector] public BoxCollider editorRaycastPlane;

		#region Core
		void Awake()
		{
			//If no default root is specified then let this gameobject be the root
			if (!root)
				root = transform;

			//Create an extremely large plane colider that is used only for editor raycasting
			// mapPlane = new Plane(Vector3.up, transform.position);
			// mapPlane.SetNormalAndPosition(Vector3.up, transform.position);
			editorRaycastPlane = gameObject.AddComponent<BoxCollider>();
			editorRaycastPlane.center = transform.position;			//Position
			editorRaycastPlane.size = new Vector3(maxMapSize, 0, maxMapSize);	//Size
			editorRaycastPlane.isTrigger = true;
			editorRaycastPlane.hideFlags = HideFlags.HideAndDontSave | HideFlags.HideInInspector;	//Hide
		}

		#endregion

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