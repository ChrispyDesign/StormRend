using UnityEngine;

namespace StormRend.Systems.Mapping
{
	[RequireComponent(typeof(SpriteRenderer))]
	public class TileHighlight : MonoBehaviour
	{
		SpriteRenderer spriteRenderer;
		
		void Start()
		{
			spriteRenderer = GetComponent<SpriteRenderer>();
		}

		public void SetColor(TileHighlightColor setting)
		{
			spriteRenderer.color = setting.color;
		}
		public void SetColor(Color color)
		{
			spriteRenderer.color = color;
		}
	}
}