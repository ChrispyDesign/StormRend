using System;
using UnityEngine;

namespace StormRend.MapSystems.Tiles
{
	[Serializable, CreateAssetMenu(menuName = "StormRend/TileHighlightColor")]
	public class TileHighlightColor : ScriptableObject
	{
		public Color color;
	}
}