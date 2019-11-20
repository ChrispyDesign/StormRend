using UnityEngine;
using UnityEngine.UI;

namespace StormRend.MapSystems.Tiles
{
	[RequireComponent(typeof(SpriteRenderer))]
	public class TileHighlight : MonoBehaviour
	{
		//Properties
		public Color color 
		{
			get => sr.color;
			set => sr.color = value;
		}

		public Sprite sprite
		{
			get => sr.sprite;
			set => sr.sprite = value;
		}

		//Members
		SpriteRenderer sr;

		void Awake()
		{
			sr = GetComponent<SpriteRenderer>();
			Debug.Assert(sr, "Sprite renderer not found!");
		}

		public void Set(TileHighlightSetting setting)
		{
			sr.color = setting.color;
			sr.sprite = setting.sprite;
		}
	}
}