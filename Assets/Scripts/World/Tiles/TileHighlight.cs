using UnityEngine;

namespace StormRend.MapSystems.Tiles
{
	[RequireComponent(typeof(SpriteRenderer))]
	public class TileHighlight : MonoBehaviour
	{
		public SpriteRenderer sr;
		public Color color 
		{
			get => sr.color;
			set => sr.color = value;
		}

		void Awake()
		{
			sr = GetComponent<SpriteRenderer>();
			Debug.Assert(sr, "Sprite renderer not found!");
		}

		// public void SetColor(TileHighlightColor highlightColor)
		// {
		// 	sr.color = highlightColor.color;
		// }
		// public void SetColor(Color color)
		// {
		// 	sr.color = color;
		// }
		// public void Clear()
		// {
		// 	sr.color = Color.clear;
		// }
	}
}