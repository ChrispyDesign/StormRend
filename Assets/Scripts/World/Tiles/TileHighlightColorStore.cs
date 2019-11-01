using StormRend.Utility.Attributes;
using UnityEngine;

namespace StormRend.MapSystems.Tiles 
{ 
	public class TileHighlightColorStore : MonoBehaviour
	{
		[Header("All tile highlights objects must be placed in here")]
		public TileHighlightColor[] highlights;
   	}
}