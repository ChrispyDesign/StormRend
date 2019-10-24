using System.Collections;
using UnityEngine;

namespace StormRend.Utility
{
	public static class SRDebug
	{
		public static void PrintCollection(IEnumerable collection)
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