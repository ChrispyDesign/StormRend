using System.Collections.Generic;
using StormRend.Systems.Mapping;
using UnityEditor;
using UnityEngine;

namespace StormRend.Editors
{
	//-------------- Core ---------------
	[CustomEditor(typeof(Map))]
	public partial class MapEditor : SmartEditor
	{
		Map m;	//target
		Event e;
		Color oldHandleColor, oldGUIColor;
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
			m = target as Map;
			stamp = new GameObject("TileStamp");
			stamp.hideFlags = HideFlags.HideAndDontSave;

			CreateStyles();
		}
		void OnDisable()
		{
			if (stamp) DestroyImmediate(stamp);
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