using UnityEngine;
using UnityEngine.UI;

namespace StormRend.MapSystems.Tiles
{
	[RequireComponent(typeof(SpriteRenderer))]
	public class TileHighlight : MonoBehaviour
	{
		//Properties
		public TileHighlight hover { get; set; }
		internal TileColor color { get; private set; }

		//Members
		SpriteRenderer sr;

		void Awake()
		{
			sr = GetComponent<SpriteRenderer>();
			Debug.Assert(sr, "Sprite renderer not found!");

			//Get reference to the hover highlight
			if (transform.childCount > 0)
				hover = transform.GetChild(0).GetComponent<TileHighlight>();
			//If this object has no children it means THIS is the hover highlight
			else
				hover = this;
		}

		public void Set(TileColor color)
		{
			this.color = color;

			//A BIT HACKY BUT OK:
			//On clicking retry for some reason .Set() is called before this class is fully initialized so sr is null
			if (!sr) Awake();		
			
			sr.color = color.color;
			sr.sprite = color.sprite;
		}
	}
}