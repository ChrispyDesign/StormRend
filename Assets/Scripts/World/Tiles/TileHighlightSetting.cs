using System;
using UnityEngine;

namespace StormRend.MapSystems.Tiles
{
	/// <summary>
	/// Stores settings for tile highlight objects found on tiles
	/// </summary>
	[Serializable, CreateAssetMenu(menuName = "StormRend/TileHighlightSetting")]
	public class TileHighlightSetting : ScriptableObject
	{
		public Color color = Color.clear;
		public Sprite sprite = null;
	}
}