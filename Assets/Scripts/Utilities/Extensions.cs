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
		public static void Print(this IEnumerable collection)
		{
			Debug.Log("Printing: " + collection);
			int i = 0;
			foreach (var item in collection)
			{
				Debug.Log(i++ + ": " + item);
			}
		}
    }
}