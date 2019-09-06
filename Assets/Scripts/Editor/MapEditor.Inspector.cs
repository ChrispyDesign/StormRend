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
		bool randomizePaintDirection;


		#region Core
		public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawDefaultInspector();

            DrawOptions();

            DrawPalette();

            DrawPreviewSizeSlider();

			DrawDebugInfo();

            serializedObject.ApplyModifiedProperties();
        }

        #endregion    //Core

        #region Draws
        void DrawButtons()
        {
            //Paint, Erase buttons
            using (new EditorGUILayout.HorizontalScope(GUILayout.MinHeight(50)))
            {
            	if (GUILayout.Button(new GUIContent("Paint", "Paint tiles"), GUILayout.MinHeight(50)))
            	{
            		editMode = EditMode.Painting;
            	}
            	if (GUILayout.Button(new GUIContent("Erase", "Erase tiles"), GUILayout.MinHeight(50)))
            	{
            		editMode = EditMode.Erasing;
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

                // GUILayoutOption gridHeight = GUILayout.Height(palettePreviews.Length / (columns) * previewTileSize);
                GUILayoutOption gridHeight = GUILayout.Height(64);

                int newIndex = GUILayout.SelectionGrid(t.selectedPrefabIDX, palettePreviews, columns, gridHeight);

                if (newIndex != t.selectedPrefabIDX)
                {
                    t.selectedPrefabIDX = newIndex;
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
            using (new GUILayout.HorizontalScope())
            {
                EditorGUILayout.PrefixLabel("Randomize Direction");
                randomizePaintDirection = EditorGUILayout.Toggle(randomizePaintDirection);
            }
		}
        void DrawPreviewSizeSlider()
        {
            GUILayout.Space(5);
            using (new GUILayout.HorizontalScope())
            {
				GUILayout.Label(string.Format("Preview Size: {0:0}", previewTileSize), EditorStyles.miniLabel);
                previewTileSize = GUILayout.HorizontalSlider(previewTileSize, 32, 128);
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
					gridCursor, t.selectedPrefabIDX, isEditing),
					MessageType.None);
		}
        #endregion
    }
}