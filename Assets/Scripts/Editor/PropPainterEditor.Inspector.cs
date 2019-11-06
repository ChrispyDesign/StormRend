using StormRend.MapSystems;
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
			if (pp.rootTransform == null)
			{
				EditorGUILayout.HelpBox("You must assign the root transform for new painted instances.", MessageType.Error);
				pp.rootTransform = (Transform)EditorGUILayout.ObjectField("Root Transform", pp.rootTransform, typeof(Transform), true);
				return;
			}
			EditorGUILayout.HelpBox("Stamp: Left Click\nErase: Ctrl + Left Click\nRotate: Shift + Scroll\nBrush Size: Alt + Scroll or [ and ]\nDensity: - =\nScale: . /\nSpace: Randomize", MessageType.Info);
		}
		public override void OnPostInspector()
		{
			if (pp.prefabPalette == null || pp.prefabPalette.Length == 0)
			{
				EditorGUILayout.HelpBox("You must assign prefabs to the Prefab Pallette array.", MessageType.Error);
				return;
			}

			//Brush palette
			if (pp.prefabPalette != null && pp.prefabPalette.Length > 0)
			{
				RefreshPaletteImages(pp);
				var tileSize = 96;
				var xCount = Mathf.FloorToInt(Screen.width / tileSize + 1);
				var gridHeight = GUILayout.Height(paletteImages.Length / (xCount) * tileSize);
				var newIndex = GUILayout.SelectionGrid(pp.selectedPrefabIndex, paletteImages, xCount, EditorStyles.miniButton, gridHeight);
				if (newIndex != pp.selectedPrefabIndex)
				{
					pp.selectedPrefabIndex = newIndex;
					CreateNewStamp();
				}
			}
		}
	#endregion
	}
}