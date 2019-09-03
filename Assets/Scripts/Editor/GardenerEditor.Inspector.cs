using StormRend.Tools;
using UnityEditor;
using UnityEngine;

namespace StormRend.Editors
{
	public partial class GardenerEditor : Editor
	{
		[MenuItem("GameObject/Create Other/Instance Painter")]
		static void CreateInstancePainter()
		{
			var g = new GameObject("Instance Painter", typeof(Gardener));
			Selection.activeGameObject = g;
		}

		void RefreshPaletteImages(Gardener ip)
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

			if (gardener.rootTransform == null)
			{
				EditorGUILayout.HelpBox("You must assign the root transform for new painted instances.", MessageType.Error);
				gardener.rootTransform = (Transform)EditorGUILayout.ObjectField("Root Transform", gardener.rootTransform, typeof(Transform), true);
				return;
			}
			EditorGUILayout.HelpBox("Stamp: Left Click\nErase: Ctrl + Left Click\nRotate: Shift + Scroll\nBrush Size: Alt + Scroll or [ and ]\nDensity: - =\nScale: . /\nSpace: Randomize", MessageType.Info);
			base.OnInspectorGUI();
			if (gardener.prefabPalette == null || gardener.prefabPalette.Length == 0)
			{
				EditorGUILayout.HelpBox("You must assign prefabs to the Prefab Pallete array.", MessageType.Error);
				return;
			}
			GUILayout.Space(16);

			using (new EditorGUILayout.HorizontalScope())
			{
				EditorGUILayout.PrefixLabel("Align to Normal");
				gardener.alignToNormal = GUILayout.Toggle(gardener.alignToNormal, GUIContent.none);
			}
			using (new EditorGUILayout.HorizontalScope())
			{
				EditorGUILayout.PrefixLabel("Follow Surface");
				gardener.followOnSurface = GUILayout.Toggle(gardener.followOnSurface, GUIContent.none);
			}
			using (new EditorGUILayout.HorizontalScope())
			{
				EditorGUILayout.PrefixLabel("Randomize each Stamp");
				gardener.randomizeAfterStamp = GUILayout.Toggle(gardener.randomizeAfterStamp, GUIContent.none);
			}

			GUILayout.Space(16);
			if (gardener.prefabPalette != null && gardener.prefabPalette.Length > 0)
			{
				RefreshPaletteImages(gardener);
				var tileSize = 96;
				var xCount = Mathf.FloorToInt(Screen.width / tileSize + 1);
				var gridHeight = GUILayout.Height(paletteImages.Length / (xCount) * tileSize);
				var newIndex = GUILayout.SelectionGrid(gardener.selectedPrefabIndex, paletteImages, xCount, EditorStyles.miniButton, gridHeight);
				if (newIndex != gardener.selectedPrefabIndex)
				{
					gardener.selectedPrefabIndex = newIndex;
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