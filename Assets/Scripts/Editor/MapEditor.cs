using StormRend.Systems.Mapping;
using UnityEditor;
using UnityEngine;

namespace StormRend.Editors
{
	//-------------- Core ---------------
	[CustomEditor(typeof(Map))]
	public partial class MapEditor : SmartEditor
	{
		Vector3 gridCursor;
		GameObject stamp;

		Map t;
		Event e;

		GUIStyle style;

		#region Cores
		[MenuItem("GameObject/StormRend/Map", false, 10)]
		static void CreateNewMap(MenuCommand menuCommand)
		{
			var newMapObj = new GameObject("Map", typeof(Map));
			GameObjectUtility.SetParentAndAlign(newMapObj, menuCommand.context as GameObject);
			Undo.RegisterCreatedObjectUndo(newMapObj, "Create StormRend Map");
			Selection.activeObject = newMapObj;
		}
		void OnEnable()
		{
			t = target as Map;
			stamp = new GameObject("TileStamp");
			stamp.hideFlags = HideFlags.HideAndDontSave;

			CreateStyles();

			Undo.undoRedoPerformed += OnUndoRedo;
		}
		void OnDisable()
		{
			if (stamp) DestroyImmediate(stamp);

			Undo.undoRedoPerformed += OnUndoRedo;
		}
		#endregion

		void CreateStyles()
		{
			style = new GUIStyle();
			style.fontSize = 15;
			style.fontStyle = FontStyle.Bold;
		}
	}
}