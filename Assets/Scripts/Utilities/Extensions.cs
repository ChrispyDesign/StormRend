/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

using System;
using System.Collections;
using UnityEngine;

namespace StormRend.Utility
{
	public static class UnityExtensions
    {
        /// <summary>
        /// Check if a layer is in a layermask
        /// </summary>
        public static bool Contains(this LayerMask mask, int layer)
        {
            return mask == (mask | (1 << layer));
        }

		/// <summary>
		/// Check if an object is in an array
		/// </summary>
		public static bool Contains(this Array array, object item)
		{
			foreach (var i in array)
				if (i == item)
					return true;
			return false;
		}

		/// <summary>
		/// Debug logs any IEnumerable collection 
		/// </summary>
		public static void Print(this IEnumerable collection, string description = null)
		{
			if (description != null)
				Debug.Log(description);
			else
				Debug.LogFormat("[{0}]", collection);

			int i = 0;
			foreach (var item in collection)
			{
				Debug.Log(i++ + ": " + item);
			}
		}

		/// <summary>
		/// Convert Vector2 to Vector2Int
		/// </summary>
		public static Vector2Int ToVector2Int(this Vector2 vector)
		{
			return new Vector2Int((int)vector.x, (int)vector.y);
		}
    }
}