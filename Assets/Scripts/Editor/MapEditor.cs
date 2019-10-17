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
		Vector3 gridCursor;
		GameObject stamp;

		Map m;
		Event e;

		GUIStyle style;

		#region Cores
		[MenuItem("GameObject/StormRend/Map", false, 10)]
		static void CreateGameObject(MenuCommand menuCommand)
		{
			var newo = new GameObject("Map", typeof(Map));
			GameObjectUtility.SetParentAndAlign(newo, menuCommand.context as GameObject);
			Undo.RegisterCreatedObjectUndo(newo, "Create StormRend Map");
			Selection.activeObject = newo;
		}
		void OnEnable()
		{
			m = target as Map;
			stamp = new GameObject("TileStamp");
			stamp.hideFlags = HideFlags.HideAndDontSave;

			//Create a internal style for this inspector to use
			CreateStyles();

			//Prevent a blank stamp from show on startup
			CreateStamp();		

			//Register events
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

		#region Utility
		void AutoConnectNeighbourTiles(Tile subject, bool connectDiagonals = false, float tolerance = 0.1f)
		{
			//Find tiles within range (ie. within the distance of the map's tilesize)
			const float adjDist = 1f , diagDist = 1.414213f;

			foreach (var t in m.tiles)
			{
				//Adjacent
				float dist = Vector3.Distance(subject.transform.position, t.transform.position);

				if ((dist - (adjDist * m.tileSize - tolerance)) * ((adjDist * m.tileSize + tolerance) - dist) >= 0)
				{
					subject.Connect(t);
				}
				//Diagonals
				if (connectDiagonals)
				{
					if ((dist - (diagDist * m.tileSize - tolerance)) * ((diagDist * m.tileSize + tolerance) - dist) >= 0)
					{
						subject.Connect(t);
					}
				}
			}
		}
		#endregion
	}
}