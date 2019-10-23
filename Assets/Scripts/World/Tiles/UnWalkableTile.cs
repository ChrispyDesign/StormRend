using UnityEngine;
using UnityEngine.EventSystems;

namespace StormRend.MapSystems.Tiles
{
	public class UnWalkableTile : Tile
	{
		const float lily = 0.01f;
		void OnDrawGizmos()
		{
			//Draw an X over the tile in editor
			var oldColor = Gizmos.color;
			Gizmos.color = Color.red * 0.9f;
			var min = rend.bounds.min;
			var max = rend.bounds.max;
			var flatbox = new Vector3(rend.bounds.size.x, lily, rend.bounds.size.z);
			Gizmos.DrawWireCube(transform.position, flatbox);
			var topLeft = new Vector3(min.x, lily, max.z);
			var bottomRight = new Vector3(max.x, lily, min.z);
			var topRight = new Vector3(max.x, lily, max.z);
			var bottomLeft = new Vector3(min.x, lily, min.z);
			Gizmos.DrawLine(bottomLeft, topRight);
			Gizmos.DrawLine(topLeft, bottomRight);
			Gizmos.color = oldColor;
		}
	}
}