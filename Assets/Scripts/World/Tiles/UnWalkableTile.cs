/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

using UnityEngine;
using UnityEngine.EventSystems;

namespace StormRend.MapSystems.Tiles
{
	public class UnWalkableTile : Tile, IPointerEnterHandler, IPointerExitHandler
	{
		const float lily = 0.01f;	//A little y
		void OnDrawGizmos()
		{
			//Draw an X over the tile in editor
			var pos = transform.position;
			var oldColor = Gizmos.color;
			Gizmos.color = Color.black * 0.9f;
			var min = rend.bounds.min;
			var max = rend.bounds.max;
			var flatbox = new Vector3(rend.bounds.size.x, lily, rend.bounds.size.z);
			Gizmos.DrawWireCube(transform.position, flatbox);
			var topLeft = new Vector3(min.x, pos.y + lily, max.z);
			var bottomRight = new Vector3(max.x, pos.y + lily, min.z);
			var topRight = new Vector3(max.x, pos.y + lily, max.z);
			var bottomLeft = new Vector3(min.x, pos.y + lily, min.z);
			Gizmos.DrawLine(bottomLeft, topRight);
			Gizmos.DrawLine(topLeft, bottomRight);
			Gizmos.color = oldColor;
		}

		public new void OnPointerEnter(PointerEventData eventData)
		{
			//Cannot highlight unwalkable tiles
			//Confusing to the user
		}

		public new void OnPointerExit(PointerEventData eventData)
		{
			//Cannot highlight unwalkable tiles
			//Confusing to the user
		}
	}
}