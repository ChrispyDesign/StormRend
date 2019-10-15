using System;
using UnityEngine;

namespace StormRend.Systems.Mapping
{
	[Serializable, CreateAssetMenu(menuName = "StormRend/TileHighlightColor")]
	public class TileHighlightColor : ScriptableObject
	{
		public Color color;
	}
}