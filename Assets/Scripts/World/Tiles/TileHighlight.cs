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

		public void SetColor(TileHighlightColor setting)
		{
			sr.color = setting.color;
		}
		public void SetColor(Color color)
		{
			sr.color = color;
		}
	}
}