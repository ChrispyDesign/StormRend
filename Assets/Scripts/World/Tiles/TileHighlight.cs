using UnityEngine;

namespace StormRend.MapSystems.Tiles
{
	[RequireComponent(typeof(SpriteRenderer))]
	public class TileHighlight : MonoBehaviour
	{
		SpriteRenderer sr;
		public Color color 
		{
			get => sr.color;
		}

		void Awake()
		{
			sr = GetComponent<SpriteRenderer>();
			Debug.Assert(sr, "Sprite renderer not found!");
		}

		public void SetColor(TileHighlightColor highlightColor)
		{
			sr.color = highlightColor.color;
		}
		public void SetColor(Color color)
		{
			sr.color = color;
		}
		public void Clear()
		{
			if (Tile.highlightColors.TryGetValue("None", out TileHighlightColor highlightColor))
				SetColor(highlightColor);
		}
	}
}