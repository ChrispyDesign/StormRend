using System;
using UnityEngine;

namespace StormRend.MapSystems.Tiles
{
	/// <summary>
	/// Stores settings for tile highlight objects found on tiles
	/// </summary>
	[Serializable, CreateAssetMenu(menuName = "StormRend/TileColor", fileName = "TileColor")]
	public class TileColor : ScriptableObject
	{
		public Color color = Color.clear;
		public Sprite sprite = null;
	}
}