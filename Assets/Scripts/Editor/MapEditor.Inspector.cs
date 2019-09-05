using System;
using StormRend.Systems.Mapping;
using UnityEditor;
using UnityEngine;

namespace StormRend.Editors
{
    //------------ Inspector ---------------
    public partial class MapEditor : Editor
    {
        float previewTileSize = 128;
        Texture2D[] palettePreviews;
		// GUIContent[] palettePreviewGUIContents;

        #region Core
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawDefaultInspector();

            // DrawButtons();

            DrawPalette();

            DrawPaletteSizeSlider();

			DrawDebugInfo();

            serializedObject.ApplyModifiedProperties();
        }

        #endregion    //Core

        void DrawButtons()
        {
            //Paint, Erase buttons
            using (new EditorGUILayout.HorizontalScope(GUILayout.MinHeight(50)))
            {
            	if (GUILayout.Button(new GUIContent("Paint", "Paint tiles"), GUILayout.MinHeight(50)))
            	{
            		mode = PaintMode.Painting;
            	}
            	if (GUILayout.Button(new GUIContent("Erase", "Erase tiles"), GUILayout.MinHeight(50)))
            	{
            		mode = PaintMode.Erasing;
            	}
            }
        }
        void DrawPalette()
        {
            //Palette
            if (t.isPaletteActive)
            {
				RefreshPalettePreviews(t);

                int columns = Mathf.FloorToInt(Screen.width / previewTileSize + 1);

                GUILayoutOption gridHeight = GUILayout.Height(palettePreviews.Length / (columns) * previewTileSize);

                int newIndex = GUILayout.SelectionGrid(t.selectedPrefabIDX, palettePreviews, columns, gridHeight);

                if (newIndex != t.selectedPrefabIDX)
                {
                    t.selectedPrefabIDX = newIndex;
                    CreateStamp();
                }
            }
            else
            {
                EditorGUILayout.HelpBox("Assign tile prefabs to palette", MessageType.Error);
                return;
            }
        }
        private void DrawPaletteSizeSlider()
        {
            using (new GUILayout.HorizontalScope())
            {
				GUILayout.Label(string.Format("Preview Size: {0:0}", previewTileSize), EditorStyles.miniLabel);
                previewTileSize = GUILayout.HorizontalSlider(previewTileSize, 32, 128);
            }
        }

		void DrawDebugInfo()
		{
			EditorGUILayout.HelpBox(
				string.Format(
					"Snapped Cursor: {0}\nselectedPrefabIndex: {1}\n",
					snappedCursor,
					t.selectedPrefabIDX), 
				MessageType.None);
		}

        void RefreshPalettePreviews(Map map)
        {
			var paletteCount = map.palette.Length;

			if (palettePreviews == null || palettePreviews.Length != paletteCount)
			{
				palettePreviews = new Texture2D[paletteCount];
				for (var i = 0; i < paletteCount; ++i)
					palettePreviews[i] = AssetPreview.GetAssetPreview(map.palette[i]);
			}

			// if (palettePreviewGUIContents == null || palettePreviewGUIContents.Length != paletteCount)
			// {
			// 	//Refresh GUI Contents
			// 	palettePreviewGUIContents = new GUIContent[paletteCount];
			// 	for (var i = 0; i < paletteCount; ++i)
			// 	{
			// 		palettePreviewGUIContents[i].text = map.palette[i].name;
			// 		palettePreviewGUIContents[i].tooltip = "Tile type: " + map.palette[i].GetComponentInChildren<Tile>().GetType().Name;
			// 		palettePreviewGUIContents[i].image = AssetPreview.GetAssetPreview(map.palette[i]) as Texture2D;
			// 	}
			// }
        }
    }
}