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

		public TileHighlight hover { get; set; }

		//Members
		SpriteRenderer sr;

		void Awake()
		{
			sr = GetComponent<SpriteRenderer>();
			Debug.Assert(sr, "Sprite renderer not found!");

			//Get refernce to the hover highlight
			if (transform.childCount > 0)
				hover = transform.GetChild(0).GetComponent<TileHighlight>();
			//If this object has no children it means THIS is the hover highlight
			else
				hover = this;
		}

		public void Set(TileHighlightSetting setting)
		{
			sr.color = setting.color;
			sr.sprite = setting.sprite;
		}
	}
}