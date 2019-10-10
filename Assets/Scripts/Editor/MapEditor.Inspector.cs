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
			DrawDebugInfo();

			using (new EditorGUILayout.HorizontalScope())
			{
				EditorGUILayout.PrefixLabel(new GUIContent("Bounds Type", "The type of method to detect collisions"));
				boundsType = (BoundsType)EditorGUILayout.EnumPopup(boundsType);
			}
		}
		public override void OnPostInspector()
		{
			DrawPreviewSizeSlider();
			DrawPalette();
			DrawOptions();
		}
		#endregion    //Core

		#region Draws
		void DrawPalette()
		{
			//Randomize direction
			using (new GUILayout.HorizontalScope())
			{
				EditorGUILayout.PrefixLabel("Randomize Tile Direction");
				randomizePaintDirection = EditorGUILayout.Toggle(randomizePaintDirection);
			}

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
				EditorGUILayout.HelpBox("Assign tile prefabs to this palette", MessageType.Error);
				return;
			}
		}

		void DrawOptions()
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

			//Clear
			GUILayout.Space(5);
			if (GUILayout.Button("Clear All Tiles"))
				m.DeleteAllTiles();
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
			var paletteCount = map.palette.Length;

			if (palettePreviews == null || palettePreviews.Length != paletteCount)
			{
				palettePreviews = new Texture2D[paletteCount];
				for (var i = 0; i < paletteCount; ++i)
					palettePreviews[i] = AssetPreview.GetAssetPreview(map.palette[i]);
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