using StormRend.Tools;
using UnityEditor;
using UnityEngine;

namespace StormRend.Editors
{
	public partial class LandscaperEditor : Editor
	{
		[MenuItem("GameObject/Create Other/Gardener")]
		static void CreateInstancePainter()
		{
			var g = new GameObject("Gardener", typeof(Landscaper));
			Selection.activeGameObject = g;
		}

		void RefreshPaletteImages(Landscaper ip)
		{
			if (paletteImages == null || paletteImages.Length != ip.prefabPalette.Length)
			{
				paletteImages = new Texture2D[ip.prefabPalette.Length];
				for (var i = 0; i < ip.prefabPalette.Length; i++)
					paletteImages[i] = AssetPreview.GetAssetPreview(ip.prefabPalette[i]);
			}
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			//Helpbox
			if (t.rootTransform == null)
			{
				EditorGUILayout.HelpBox("You must assign the root transform for new painted instances.", MessageType.Error);
				t.rootTransform = (Transform)EditorGUILayout.ObjectField("Root Transform", t.rootTransform, typeof(Transform), true);
				return;
			}
			EditorGUILayout.HelpBox("Stamp: Left Click\nErase: Ctrl + Left Click\nRotate: Shift + Scroll\nBrush Size: Alt + Scroll or [ and ]\nDensity: - =\nScale: . /\nSpace: Randomize", MessageType.Info);

			base.OnInspectorGUI();	//Default Inspector Draw

			if (t.prefabPalette == null || t.prefabPalette.Length == 0)
			{
				EditorGUILayout.HelpBox("You must assign prefabs to the Prefab Pallete array.", MessageType.Error);
				return;
			}
			GUILayout.Space(16);

			//Options
			using (new EditorGUILayout.HorizontalScope())
			{
				EditorGUILayout.PrefixLabel("Align to Normal");
				t.alignToNormal = GUILayout.Toggle(t.alignToNormal, GUIContent.none);
			}
			using (new EditorGUILayout.HorizontalScope())
			{
				EditorGUILayout.PrefixLabel("Follow Surface");
				t.followOnSurface = GUILayout.Toggle(t.followOnSurface, GUIContent.none);
			}
			using (new EditorGUILayout.HorizontalScope())
			{
				EditorGUILayout.PrefixLabel("Randomize each Stamp");
				t.randomizeAfterStamp = GUILayout.Toggle(t.randomizeAfterStamp, GUIContent.none);
			}
			GUILayout.Space(16);

			//Brush palette
			if (t.prefabPalette != null && t.prefabPalette.Length > 0)
			{
				RefreshPaletteImages(t);
				var tileSize = 96;
				var xCount = Mathf.FloorToInt(Screen.width / tileSize + 1);
				var gridHeight = GUILayout.Height(paletteImages.Length / (xCount) * tileSize);
				var newIndex = GUILayout.SelectionGrid(t.selectedPrefabIndex, paletteImages, xCount, EditorStyles.miniButton, gridHeight);
				if (newIndex != t.selectedPrefabIndex)
				{
					t.selectedPrefabIndex = newIndex;
					// variations = ip.SelectedPrefab.GetComponent<Variations>();
					// if (variationsEditor != null)
						// DestroyImmediate(variationsEditor);
					// if (variations != null)
						// variationsEditor = Editor.CreateEditor(variations);
					CreateNewStamp();
				}
				// GUILayout.Space(16);
				// if (variationsEditor == null)
				// {
				// 	if (GUILayout.Button("Add Variations"))
				// 	{
				// 		// variations = ip.SelectedPrefab.AddComponent<Variations>();
				// 		// variationsEditor = Editor.CreateEditor(variations);
				// 	}
				// }
				// else
				// {
				// 	variationsEditor.OnInspectorGUI();
				// }

			}
		}
	}
}