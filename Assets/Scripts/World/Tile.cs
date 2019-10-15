using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace StormRend.Systems.Mapping
{
	/// <summary>
	/// Base tile class. Holds a list of connections to neighbouring tiles. 
	/// A tile prefab should be structured:
	/// BaseTile
	/// + Highlight: TileHighlight, SpriteRenderer.
	/// + Any toppers (prefab variants)
	/// + Any other extra objects
	/// </summary>
	public abstract class Tile : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler 
	{
		public static List<TileHighlightColor> tileHighlights = new List<TileHighlightColor>();

		public Map owner;
		public HashSet<Tile> connections = new HashSet<Tile>();

		[Header("Costs")]
		public float cost = 1;
		public float G = float.MaxValue;
		public float H = float.MaxValue;
		public float F = 0;

		//Properties
		public TileHighlight highlight => _highlight;

		//Members
		TileHighlight _highlight;

	#region Core
		void Start()
		{
			_highlight = GetComponentInChildren<TileHighlight>();
			//Create a highlight object if nothing found
			if (!_highlight)
			{
				var go = new GameObject("Highlight");
				_highlight = go.AddComponent<TileHighlight>();
				go.transform.SetParent(this.transform);
			}	
		}
		public void Connect(Tile to) => connections.Add(to);
		public void Disconnect(Tile from) => connections.Remove(from);
		public void DisconnectAll() => connections.Clear();
	#endregion

	#region Event System Interface Implementations
		public void OnPointerClick(PointerEventData eventData)
		{
			throw new System.NotImplementedException();
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			throw new System.NotImplementedException();
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			throw new System.NotImplementedException();
		}
	#endregion
	}
}
