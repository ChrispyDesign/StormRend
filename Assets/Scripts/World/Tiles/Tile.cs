using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace StormRend.MapSystems.Tiles
{
	/// <summary>
	/// Base tile class. Holds a list of connections to neighbouring tiles.
	/// A tile prefab should be structured:
	/// BaseTile
	/// + Highlight: TileHighlight, SpriteRenderer.
	/// + Any toppers (prefab variants)
	/// + Any other extra objects
	/// </summary>
	public abstract class Tile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		//Static
		public static TileHighlightColor[] highlightColors;
		public static TileHighlightColor FindHighlightColor(string highlightName)
		{
			foreach (var h in highlightColors)
			{
				//Highlight color found in static list of highlights
				if (h.name == highlightName)
					return h;
			}
			//Nothing found return null
			return null;
		}

		//Inspector
		public float cost = 1;
		internal float G = float.MaxValue;
		internal float H = float.MaxValue;
		internal float F = 0;

		//Properties
		public TileHighlight highlight => _highlight;

		//Members
		public HashSet<Tile> connections = new HashSet<Tile>();
		// [HideInInspector]
		 public Map owner;
		public TileHighlight _highlight;
		public Color oldColor;

	#region Core
		void Awake()
		{
			highlightColors = Resources.FindObjectsOfTypeAll<TileHighlightColor>();
			foreach (var c in highlightColors)
				Debug.Log(c);
		}
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

	#region Uncertain Methods
		/// <summary>
		/// Returns an adjacent tile in a certain direction.
		/// If no tile found then return null.
		/// This should work with diagonals too ie. direction = {-1, 1} = forward, left
		/// </summary>
		public Tile FindConnectedTile(Vector2Int direction, float tolerance = 0.1f)
		{
			//Vector2Int crude lossy normalize?
			direction.Clamp(new Vector2Int(-1, -1), Vector2Int.one);

			const float adjDist = 1f;//, diagDist = 1.414213f;

			//Determine where to scan for a connected tile
			var targetTilePos = transform.position + new Vector3(adjDist * owner.tileSize * direction.x, 0, adjDist * owner.tileSize * direction.y);

			//Loop through all connected tiles and see there are any within tolerance
			foreach (var c in connections)
			{
				var dist = Vector3.Distance(c.transform.position, targetTilePos);
				if (dist < tolerance)
					return c;
			}
			return null;
		}
	#endregion

	#region Event System Interface Implementations
		// public void OnPointerClick(PointerEventData eventData)
		// {
		// 	Debug.LogFormat("{0}.OnPointerClick()");
		// }

		public void OnPointerEnter(PointerEventData eventData)
		{
			// eventData.hovered
			// highlight.gameObject.SetActive(true);
			oldColor = highlight.color;
			highlight.SetColor(new Color(1, 1, 1, 0.5f));

			// Debug.LogFormat("{0}.Hover", name);
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			highlight.SetColor(oldColor);
			// if (!oldColor.Equals(Color.clear))
				// highlight.gameObject.SetActive(false);

			// Debug.LogFormat("{0}.UnHover", name);
		}
	#endregion
	}
}
