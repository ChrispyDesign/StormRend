using System.Collections.Generic;
using StormRend.Units;
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
		public float cost = 1;
		internal float G = float.MaxValue;
		internal float H = float.MaxValue;
		internal float F = 0;

		//Properties
		public Map owner => Map.current;

		//Members
		[ReadOnlyField] public List<Tile> connections = new List<Tile>();	//List because HashSets don't serialize
		[HideInInspector, SerializeField] protected Renderer rend;
		TileHighlightColor internalColor;		//This kinda acts like a more reliable oldColor value
		TileHighlightColor clearColor;
		TileHighlight highlight;

	#region Core
		void OnValidate()	//Need to get the renderer in editor for gizmos to work
		{
			rend = GetComponent<Renderer>();
		}
		void Awake()
		{
			LoadStaticHighlightColors();    //NOTE! Awake is too early sometimes? Which means it doesn't always grab all the Tile Highlight Colors
			SetupTileHighlightObject();
			SetupInternalColours();
		}

		public void Connect(Tile to) => connections.Add(to);
		public bool Disconnect(Tile from) => connections.Remove(from);
		public bool Contains(Tile t) => connections.Contains(t);
		public void DisconnectAll() => connections.Clear();
		public void SetColor(TileHighlightColor tileHighlightColor)
		{
			internalColor = tileHighlightColor;
			highlight.color = internalColor.color;
		}
		public void SetColor(Color color)
		{
			internalColor.color = color;
			highlight.color = internalColor.color;
		}
		public void ClearColor()
		{
			internalColor = clearColor;
			highlight.color = internalColor.color;
		}
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
				// var foundHighlights = Resources.LoadAll("", typeof(TileHighlightColor)) as TileHighlightColor[];
				foreach (var fh in foundHighlights)
				{
					// Debug.Log("Loading Tile Highlight Color: " + fh.name);
					highlightColors.Add(fh.name, fh);
				}
				highlightsScanned = true;
			}
		}
		void SetupTileHighlightObject()
		{
			highlight = GetComponentInChildren<TileHighlight>();

			//Create a highlight object if nothing found
			if (highlight) return;
			var go = new GameObject("Highlight");
			highlight = go.AddComponent<TileHighlight>();
			go.transform.SetParent(this.transform);
		}

		void SetupInternalColours()
		{
			//Setup internal tile highlight color
			internalColor = ScriptableObject.CreateInstance<TileHighlightColor>();
			clearColor = ScriptableObject.CreateInstance<TileHighlightColor>();
			clearColor.color = Color.clear;		//Clear
			internalColor = clearColor;			//Set default to clear
		}
	#endregion

	#region Utility
		/// <summary>
		/// Returns an adjacent tile in a certain direction.
		/// Will only return immediately adjacent diagonal tiles.
		/// If no tile found then return null.
		/// </summary>
		public bool TryGetTile(Vector2Int direction, out Tile tile, bool diagonal = false, float tolerance = 0.1f)
		{
			const float adjacentDist = 1f; const float diagDist = 1.414213f;

			//ADJACENT
			//Determine where to scan for a tile
			var targetTilePos = transform.position +
				new Vector3(direction.x * owner.tileSize * adjacentDist, 0, direction.y * owner.tileSize * adjacentDist);

			//Loop through all connected tiles and see if there are any within tolerance
			foreach (var t in owner.tiles)
			{
				//Ignore variations in height
				var flattenedTilePosition = new Vector3(t.transform.position.x, this.transform.position.y, t.transform.position.z);
				var dist = Vector3.Distance(flattenedTilePosition, targetTilePos);
				if (dist < tolerance)
				{
					//Tile found
					tile = t;
					return true;
				}
			}

			//DIAGONALS
			if (diagonal)
			{
				//Determine where to scan for the diagonal tile
				//NOTE: This only works with immediately adjacent tiles
				targetTilePos = transform.position + 
					new Vector3(direction.x * owner.tileSize * diagDist, 0, direction.y * owner.tileSize * diagDist);

				//Loop through all connected tiles and see if there are any within tolerance
				foreach (var t in owner.tiles)
				{
					//Ignore variations in height
					var flattenedTilePosition = new Vector3(t.transform.position.x, this.transform.position.y, t.transform.position.z);
					var dist = Vector3.Distance(flattenedTilePosition, targetTilePos);
					if (dist < tolerance)
					{
						//Tile found
						tile = t;
						return true;
					}
				}
			}

			//Nothing found
			tile = null;
			return false;
		}
	#endregion

	#region Event System Interface Implementations
		public void OnPointerEnter(PointerEventData eventData)
		{
			// if (hoverHighlight) highlight.SetColor(hoverHighlight);

			if (highlightColors.TryGetValue("Hover", out TileHighlightColor color))
				highlight.color = color.color;
		}
		public void OnPointerExit(PointerEventData eventData)
		{
			//Reset back
			highlight.color = internalColor.color;
		}
	#endregion
	}
}
