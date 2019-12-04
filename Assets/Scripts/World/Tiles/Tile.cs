using System.Collections.Generic;
using StormRend.Systems;
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
		public static Dictionary<string, TileColor> highlightColors { get; private set; } = new Dictionary<string, TileColor>();

		//Inspector
		[SerializeField] AudioClip onHoverSFX = null;
		[Range(0f, 1f), SerializeField] float SFXVolume = 0.25f;

		[Tooltip("If not set will default to 'Hover' highlight or clear")]
		[SerializeField] TileColor hoverHighlight = null;
		public float cost = 1;

		//Properties
		public Map owner => Map.current;

		//Members
		[ReadOnlyField] public List<Tile> connections = new List<Tile>();   //List because HashSets don't serialize
		[HideInInspector, SerializeField] protected Renderer rend = null;

		//This avoids tile highlight issues when cursor unhovers
		TileColor mainColor = null;       //The current default normal color of this tile
		TileColor clearColor = null;
		TileHighlight highlight = null;
		AudioSource audioSource = null;

		//Other
		internal float G = float.MaxValue;
		internal float H = float.MaxValue;
		internal float F = 0;

		//Inits
		void OnValidate()   //Need to get the renderer in editor for gizmos to work
		{
			rend = GetComponent<Renderer>();
		}
		void Awake()
		{
			LoadStaticHighlightColors();    //NOTE! Awake is too early sometimes? Which means it doesn't always grab all the Tile Highlight Colors
			SetupTileHighlightObjects();
			SetupDefaultHighlightSettings();

			//Get the general purpose
			audioSource = GameDirector.current.sfxAudioSource;
		}
		/// <summary>
		/// To create extra tile highlights: Create Asset Menu >>> Tile Highlight Color.
		/// The can be placed anywhere in the project and this function will find them on Awake()
		/// </summary>
		static void LoadStaticHighlightColors()
		{
			if (!highlightsScanned)
			{
				var foundHighlights = Resources.FindObjectsOfTypeAll<TileColor>();
				// var foundHighlights = Resources.LoadAll("", typeof(TileHighlightColor)) as TileHighlightColor[];
				foreach (var fh in foundHighlights)
				{
					// Debug.Log("Loading Tile Highlight Color: " + fh.name);
					highlightColors.Add(fh.name, fh);
				}
				highlightsScanned = true;
			}
		}
		void SetupTileHighlightObjects()
		{
			highlight = GetComponentInChildren<TileHighlight>();
			if (highlight) return;

			//Create a highlight object if nothing found
			//MAIN
			var mainHighlight = new GameObject("MainHighlight", typeof(TileHighlight));
			highlight = mainHighlight.GetComponent<TileHighlight>();
			mainHighlight.transform.SetParent(this.transform);
			//HOVER
			var hoverHighlight = new GameObject("HoverHighlight", typeof(TileHighlight));
			highlight.hover = hoverHighlight.GetComponent<TileHighlight>();
			hoverHighlight.transform.SetParent(mainHighlight.transform);
		}
		void SetupDefaultHighlightSettings()
		{
			//Setup initial inernal tile highlight color
			mainColor = clearColor = ScriptableObject.CreateInstance<TileColor>();

			//MAIN
			highlight.Set(mainColor);

			//HOVER
			highlight.hover.Set(mainColor);
		}

		//Connections
		public void Connect(Tile to) => connections.Add(to);
		public bool Disconnect(Tile from) => connections.Remove(from);
		public bool Contains(Tile t) => connections.Contains(t);
		public void DisconnectAll() => connections.Clear();

		//Highlights
		/// <summary>
		/// Sets this tile's temporary highlight while remembering the default color
		/// </summary>
		public void SetHighlight(TileColor thlSetting)
		{
			mainColor = thlSetting;
			highlight.Set(mainColor);
		}
		/// <summary>
		/// Returns the tile back to it's default color
		/// </summary>
		public void ClearHighlight()
		{
			mainColor = clearColor;
			highlight.Set(mainColor);
		}

		#region Utility
		/// <summary>
		/// Get an imaginary projected tile position from this tile
		/// </summary>
		public Vector3 GetProjectedTilePos(Vector2Int direction, bool diagonal = false)
		{
			const float adjacentDist = 1f; const float diagDist = 1.414213f;

			//Determine where to scan for a tile
			if (diagonal)
				return transform.position + new Vector3(direction.x * owner.tileSize * diagDist, 0, direction.y * owner.tileSize * diagDist);
			else
				return transform.position + new Vector3(direction.x * owner.tileSize * adjacentDist, 0, direction.y * owner.tileSize * adjacentDist);
		}

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

		//HOVER HIGHLIGHTING ONLY
		public void OnPointerEnter(PointerEventData eventData)
		{
			//Set default if no color specifically set at startup
			if (!hoverHighlight && highlightColors.TryGetValue("Hover", out TileColor color))
				hoverHighlight = color;

			//Only set Hover if the current tile's main highlight is not set to a default color ie. the tile is not being active ??

			//Set hover
			if (highlight.color != clearColor)
			{
				highlight.hover.Set(hoverHighlight);

				//Hover sound
				audioSource.PlayOneShot(onHoverSFX, SFXVolume);
			}

		}
		public void OnPointerExit(PointerEventData eventData)
		{
			//Reset back
			highlight.hover.Set(clearColor);
		}
		#endregion
	}
}
