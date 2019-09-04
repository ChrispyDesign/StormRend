using System.Collections.Generic;
using StormRend.Systems.Mapping;
using UnityEditor;
using UnityEngine;

namespace StormRend.Editors
{
	[CustomEditor(typeof(Map))]
	public partial class MapEditor : Editor
	{
		Color[] paletteAssetPreviews;
		GameObject stamp;
		List<Tile> eraseList = new List<Tile>();
		Vector3 snappedCursor;
		Map t;
		Event e;

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
		}
		void OnDisable()
		{
			if (stamp) DestroyImmediate(stamp);
		}

	#endregion

		void CreateStamp()
		{
			//Destoy stamp's child
			while (stamp.transform.childCount > 0)
				DestroyImmediate(stamp.transform.GetChild(0).gameObject);

			//Recreate

		}

		void ExecuteErase()
		{

		}

		void ExecuteStamp()
		{

		}
	}
}