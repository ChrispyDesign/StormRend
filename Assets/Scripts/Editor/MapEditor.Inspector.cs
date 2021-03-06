/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

using StormRend.Enums;
using StormRend.MapSystems;
using UnityEditor;
using UnityEngine;

namespace StormRend.Editors
{
	//------------ Inspector ---------------
	public partial class MapEditor : SmartEditor
	{
		BoundsType tileBoundsType = BoundsType.RendererBounds;

		bool randomPaintDirectionOn;

		bool randomVerticalStaggerOn;

		float previewTileSize = 128;
		Texture2D[] palettePreviews;
		bool connectDiagonals;
		bool showConnections;


		#region Core
		public override string[] propertiesToExclude => new[] { "m_Script" };

		public override void OnPreInspector()
		{
			// DrawDebugInfo();
			DrawHelp();

			using (new EditorGUILayout.HorizontalScope())
			{
				EditorGUILayout.PrefixLabel(new GUIContent("Bounds Type", "The type of method to detect collisions"));
				tileBoundsType = (BoundsType)EditorGUILayout.EnumPopup(tileBoundsType);
			}
		}
		public override void OnPostInspector()
		{
			DrawRandomizeOptions();
			DrawPalette();
			DrawConnectionOptions();
			// DrawPreviewSizeSlider();
		}
		#endregion    //Core

		#region Draws
		void DrawHelp()
		{
			EditorGUILayout.HelpBox("Paint: Left Click\nErase: Ctrl + Left Click\nVertical Shift: Shift + Scroll Wheel", MessageType.Info, true);
		}

		void DrawRandomizeOptions()
		{
			//Randomize direction
			using (new GUILayout.HorizontalScope())
			{
				randomPaintDirectionOn = EditorGUILayout.Toggle("Randomize Direction", randomPaintDirectionOn);
			}
			using (new GUILayout.HorizontalScope())
			{
				randomVerticalStaggerOn = EditorGUILayout.Toggle("Vertical Staggering", randomVerticalStaggerOn);
				m.yOffsetRandRange = EditorGUILayout.FloatField("Random Range", m.yOffsetRandRange);
			}
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
				m.maxConnectHeightDifference = EditorGUILayout.Slider("Max Height Difference", m.maxConnectHeightDifference, 0, 3f);
			}
			using (new GUILayout.HorizontalScope())
			{
				showConnections = EditorGUILayout.Toggle("Show Connections", showConnections);
				connectDiagonals = EditorGUILayout.Toggle("Connect Diagonals", connectDiagonals);
			}
			using (new GUILayout.HorizontalScope())
			{
				if (GUILayout.Button("Connect Neighbours"))
                    ConnectAllTiles();

                if (GUILayout.Button("Clear Connections"))
					m.ClearAllTileConnections();
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