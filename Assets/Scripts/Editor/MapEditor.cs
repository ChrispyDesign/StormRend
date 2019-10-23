using StormRend.MapSystems;
using StormRend.MapSystems.Tiles;
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
			EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
		}
		void OnDisable()
		{
			if (stamp) DestroyImmediate(stamp);

			Undo.undoRedoPerformed += OnUndoRedo;
			EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
		}
		#endregion

		void CreateStyles()
		{
			style = new GUIStyle();
			style.fontSize = 15;
			style.fontStyle = FontStyle.Bold;
		}

		void OnPlayModeStateChanged(PlayModeStateChange stateChange)
		{
			switch (stateChange)
			{
				//Prevent "Some objects were not cleaned up when closing the scene" errors
				case PlayModeStateChange.ExitingEditMode:
					//Unselect map to prevent dumb errors
					if (stamp) DestroyImmediate(stamp);
					Selection.activeGameObject = null;
					break;
				//This doesn't really work anyways...
				// case PlayModeStateChange.EnteredEditMode:
				// 	Selection.activeGameObject = m.gameObject;
				// 	break;
			}
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
					//Prevents duplicates because we can't use hashsets
					if (subject.Contains(t))
					{
						Debug.LogFormat("{0} is already connected to {1}", t, subject);
						continue;
					}
					subject.Connect(t);
				}
				//Diagonals
				if (connectDiagonals)
				{
					if ((dist - (diagDist * m.tileSize - tolerance)) * ((diagDist * m.tileSize + tolerance) - dist) >= 0)
					{
						//Prevents duplicates because we can't use hashsets
						if (subject.Contains(t))
						{
							Debug.LogFormat("{0} is already connected to {1}", t, subject);
							continue;
						}
						subject.Connect(t);
					}
				}
			}
			EditorUtility.SetDirty(subject);	//Persist data in editor
		}
		#endregion
	}
}