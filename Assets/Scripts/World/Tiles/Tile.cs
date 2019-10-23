using System.Collections.Generic;
using StormRend.Utility.Attributes;
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
	[SelectionBase]
	[RequireComponent(typeof(Collider))]
	public abstract class Tile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		//Highlights
		static bool highlightsScanned = false;
		public static Dictionary<string, TileHighlightColor> highlightColors { get; private set; } = new Dictionary<string, TileHighlightColor>();

		//Inspector
		[SerializeField] TileHighlightColor hoverHighlight = null;
		public float cost = 1;
		internal float G = float.MaxValue;
		internal float H = float.MaxValue;
		internal float F = 0;

		//Properties
		public TileHighlight highlight => _highlight;
		public Color currentHighlightColor => highlight.color;

		//Members
		[ReadOnlyField] public List<Tile> connections = new List<Tile>();	//List because HashSets don't serialize
		protected Renderer rend;

		//Debugs
		[ReadOnlyField] public Map owner;
		[ReadOnlyField] public TileHighlight _highlight;
		Color oldColor;

	#region Core
		void Awake()
		{
			LoadStaticHighlightColors();
		}
		void OnValidate()	//Need to get the renderer in editor for gizmos to work
		{
			rend = GetComponent<Renderer>();
		}
		void Start()
		{
			SetupHighlight();
		}

		public void Connect(Tile to) => connections.Add(to);
		public bool Disconnect(Tile from) => connections.Remove(from);
		public bool Contains(Tile t) => connections.Contains(t);
		public void DisconnectAll() => connections.Clear();
	#endregion

	#region Inits
		/// <summary>
		/// To create extra tile highlights: Create Asset Menu >>> Tile Highlight Color.
		/// The can be placed anywhere in the project and this function will find them on Awake()
		/// </summary>
		static void LoadStaticHighlightColors()
		{
			if (!highlightsScanned)
			{
				var foundHighlights = Resources.FindObjectsOfTypeAll<TileHighlightColor>();
				foreach (var fh in foundHighlights)
				{
					// Debug.Log(fh.name);
					highlightColors.Add(fh.name, fh);
				}
				highlightsScanned = true;
			}
		}
		void SetupHighlight()
		{
			_highlight = GetComponentInChildren<TileHighlight>();

			//Create a highlight object if nothing found
			if (highlight) return;
			var go = new GameObject("Highlight");
			_highlight = go.AddComponent<TileHighlight>();
			go.transform.SetParent(this.transform);
		}
	#endregion

	#region Core
		/// <summary>
		/// Returns an adjacent tile in a certain direction.
		/// If no tile found then return null.
		/// This should work with diagonals too ie. direction = {-1, 1} = forward, left
		/// </summary>
		public bool TryGetConnectedTile(Vector2Int direction, out Tile tile, float tolerance = 0.1f)
		{
			const float adjDist = 1f;//, diagDist = 1.414213f;

			//Vector2Int crude lossy normalization?
			direction.Clamp(new Vector2Int(-1, -1), Vector2Int.one);

			//Determine where to scan for a connected tile
			var targetTilePos = transform.position +
				new Vector3(adjDist * owner.tileSize * direction.x, 0, adjDist * owner.tileSize * direction.y);

			//Loop through all connected tiles and see if there are any within tolerance
			foreach (var connectedTile in connections)
			{
				var dist = Vector3.Distance(connectedTile.transform.position, targetTilePos);
				if (dist < tolerance)
				{
					tile = connectedTile;
					return true;
				}
			}
			tile = null;
			return false;

		}
	#endregion

	#region Event System Interface Implementations
		public void OnPointerEnter(PointerEventData eventData)
		{
			oldColor = highlight.color;
			if (hoverHighlight) highlight.SetColor(hoverHighlight);
			// if (highlightColors.TryGetValue("Hover", out TileHighlightColor color))
			// 	highlight.SetColor(color);
		}
		public void OnPointerExit(PointerEventData eventData)
		{
			highlight.SetColor(oldColor);
		}
	#endregion
	}
}
