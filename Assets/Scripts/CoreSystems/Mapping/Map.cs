using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StormRend.Systems.Mapping
{
	public class Map : MonoBehaviour
	{
		[SerializeField] Transform root;
		[SerializeField] Color hoverTint = Color.yellow;
		[SerializeField] Color attackTint = Color.red;
		[SerializeField] Color moveTint = Color.blue;

        List<Tile> tiles;
		[SerializeField] float sizeX;
		[SerializeField] float sizeY;
		[SerializeField] float sizeZ;


	#region Core
		void Awake()
		{
			if (!root)
				root = transform;
		}



	#endregion

		public void ConnectNeighbourTilesByDistance(float connectRadius)
		{

		}

		public void ConnectTilesByManhattan()
		{

		}

		public void UpdateTiles()
		{

		}

		public void GetTileTerrainCost(Tile tile) {}

		public static Tile[] AStar(Map map, Tile start, Tile end)
		{
			return new Tile[0];
		}

	}
}
