using StormRend.Tools;
using UnityEditor;
using UnityEngine;

namespace StormRend.Editors
{
	public partial class PropPainterEditor : SmartEditor
	{
		[MenuItem("GameObject/StormRend/Prop Painter", false)]
		static void CreateGameObject(MenuCommand menuCommand)
		{
			var newGO = new GameObject("PropPainter", typeof(PropPainter));
			GameObjectUtility.SetParentAndAlign(newGO, menuCommand.context as GameObject);
			newGO.GetComponent<PropPainter>().rootTransform = newGO.transform;
			Undo.RegisterCreatedObjectUndo(newGO, "Create StormRend Prop Painter");
			Selection.activeObject = newGO;
		}

		void RefreshPaletteImages(PropPainter ip)
		{
			if (paletteImages == null || paletteImages.Length != ip.prefabPalette.Length)
			{
				paletteImages = new Texture2D[ip.prefabPalette.Length];
				for (var i = 0; i < ip.prefabPalette.Length; i++)
					paletteImages[i] = AssetPreview.GetAssetPreview(ip.prefabPalette[i]);
			}
		}

		#region Core
		public override string[] propertiesToExclude => new [] { "m_Script" };
		public override void OnPreInspector()
		{
			//Helpbox
			if (l.rootTransform == null)
			{
				EditorGUILayout.HelpBox("You must assign the root transform for new painted instances.", MessageType.Error);
				l.rootTransform = (Transform)EditorGUILayout.ObjectField("Root Transform", l.rootTransform, typeof(Transform), true);
				return;
			}
			EditorGUILayout.HelpBox("Stamp: Left Click\nErase: Ctrl + Left Click\nRotate: Shift + Scroll\nBrush Size: Alt + Scroll or [ and ]\nDensity: - =\nScale: . /\nSpace: Randomize", MessageType.Info);
		}
		public override void OnPostInspector()
		{
			if (l.prefabPalette == null || l.prefabPalette.Length == 0)
			{
				EditorGUILayout.HelpBox("You must assign prefabs to the Prefab Pallete array.", MessageType.Error);
				return;
			}

			//Options
			GUILayout.Space(16);
			using (new EditorGUILayout.HorizontalScope())
			{
				EditorGUILayout.PrefixLabel("Align to Normal");
				l.alignToNormal = GUILayout.Toggle(l.alignToNormal, GUIContent.none);
			}
			using (new EditorGUILayout.HorizontalScope())
			{
				EditorGUILayout.PrefixLabel("Follow Surface");
				l.followOnSurface = GUILayout.Toggle(l.followOnSurface, GUIContent.none);
			}
			using (new EditorGUILayout.HorizontalScope())
			{
				EditorGUILayout.PrefixLabel("Randomize each Stamp");
				l.randomizeAfterStamp = GUILayout.Toggle(l.randomizeAfterStamp, GUIContent.none);
			}
			GUILayout.Space(16);

			//Brush palette
			if (l.prefabPalette != null && l.prefabPalette.Length > 0)
			{
				RefreshPaletteImages(l);
				var tileSize = 96;
				var xCount = Mathf.FloorToInt(Screen.width / tileSize + 1);
				var gridHeight = GUILayout.Height(paletteImages.Length / (xCount) * tileSize);
				var newIndex = GUILayout.SelectionGrid(l.selectedPrefabIndex, paletteImages, xCount, EditorStyles.miniButton, gridHeight);
				if (newIndex != l.selectedPrefabIndex)
				{
					l.selectedPrefabIndex = newIndex;
					CreateNewStamp();
				}
			}
		}
		#endregion
	}
}