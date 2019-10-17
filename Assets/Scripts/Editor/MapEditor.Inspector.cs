using System;
using StormRend.Systems.Mapping;
using UnityEditor;
using UnityEngine;

namespace StormRend.Editors
{
	//------------ Inspector ---------------
	public partial class MapEditor : SmartEditor
	{
		public enum BoundsType { RendererBounds, ColliderBounds }

		float previewTileSize = 128;
		Texture2D[] palettePreviews;
		bool randomizePaintDirection;
		BoundsType boundsType;
		bool connectDiagonals;
		private bool showConnections;

		#region Core
		public override string[] propertiesToExclude => new[] { "m_Script" };

		public override void OnPreInspector()
		{
			// DrawDebugInfo();
			DrawHelp();

			using (new EditorGUILayout.HorizontalScope())
			{
				EditorGUILayout.PrefixLabel(new GUIContent("Bounds Type", "The type of method to detect collisions"));
				boundsType = (BoundsType)EditorGUILayout.EnumPopup(boundsType);
			}
		}
		public override void OnPostInspector()
		{
			// DrawPreviewSizeSlider();

			//Randomize direction
			using (new GUILayout.HorizontalScope())
			{
				EditorGUILayout.PrefixLabel("Randomize Direction");
				randomizePaintDirection = EditorGUILayout.Toggle(randomizePaintDirection);
			}

			DrawPalette();

			//Clear All tiles
			GUILayout.Space(5);
			if (GUILayout.Button("Clear All Tiles"))
			{
				m.DeleteAllTiles();
			}

			DrawConnectionOptions();
		}
		#endregion    //Core

		#region Draws
		void DrawHelp()
		{
			EditorGUILayout.HelpBox("Paint: Left Click\nErase: Ctrl + Left Click", MessageType.Info, true);
		}
		void DrawPalette()
		{
			//Palette
			if (m.isPaletteActive)
			{
				RefreshPalettePreviews(m);

				int columns = Mathf.FloorToInt(Screen.width / previewTileSize + 1);

				GUILayoutOption gridHeight = GUILayout.Height(palettePreviews.Length / (columns) * previewTileSize);
				// GUILayoutOption gridHeight = GUILayout.Height(64);

				int newIndex = GUILayout.SelectionGrid(m.selectedPrefabIDX, palettePreviews, columns, gridHeight);

				if (newIndex != m.selectedPrefabIDX)
				{
					m.selectedPrefabIDX = newIndex;
					CreateStamp();
				}
			}
			else
			{
				EditorGUILayout.HelpBox("Assign tile prefabs to the palette", MessageType.Error);
				return;
			}
		}

		void DrawConnectionOptions()
		{
			//Connections
			GUILayout.Space(5);
			GUILayout.Label("Connections", EditorStyles.boldLabel);
			using (new GUILayout.HorizontalScope())
			{
				EditorGUILayout.PrefixLabel("Show Connections"); showConnections = EditorGUILayout.Toggle(showConnections);
				EditorGUILayout.PrefixLabel("Connect Diagonals"); connectDiagonals = EditorGUILayout.Toggle(connectDiagonals);
			}
			using (new GUILayout.HorizontalScope())
			{
				if (GUILayout.Button("Connect Neighbours"))
				{
					foreach (var t in m.tiles)
						AutoConnectNeighbourTiles(t, connectDiagonals ? true : false, 0.2f);
				}
				if (GUILayout.Button("Clear Connections"))
				{
					m.ClearAllTileConnections();
				}
			}
		}
		void DrawPreviewSizeSlider()
		{
			GUILayout.Space(5);
			using (new GUILayout.HorizontalScope())
			{
				GUILayout.Label(string.Format("Preview Size: {0:0}", previewTileSize), EditorStyles.miniLabel);
				previewTileSize = GUILayout.HorizontalSlider(previewTileSize, 32, 256);
			}
			GUILayout.Space(5);
		}
		#endregion

		#region Assists
		void RefreshPalettePreviews(Map map)
		{
			//Refresh previews if palette has changed
			if (palettePreviews == null || palettePreviews.Length != map.palette.Length)
			{
				palettePreviews = new Texture2D[map.palette.Length];
				for (var i = 0; i < map.palette.Length; ++i)
					palettePreviews[i] = AssetPreview.GetAssetPreview(map.palette[i].gameObject);   //GetAssetPreview() must take GameObject
			}
		}
		#endregion

		#region Debugs
		void DrawDebugInfo()
		{
			EditorGUILayout.HelpBox(
				string.Format("Snapped Cursor: {0}\nselectedPrefabIndex: {1}\nisEditing: {2}",
					gridCursor, m.selectedPrefabIDX, isEditing),
					MessageType.None);
		}
		#endregion
	}
}