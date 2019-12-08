/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

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

		#region Cores
		[MenuItem("GameObject/StormRend/Map", false, 10)]
		static void CreateGameObject(MenuCommand menuCommand)
		{
			var map = new GameObject("Map", typeof(Map));
			GameObjectUtility.SetParentAndAlign(map, menuCommand.context as GameObject);
			Undo.RegisterCreatedObjectUndo(map, "Create StormRend Map");
			Selection.activeObject = map;
		}
		void OnEnable()
		{
			m = target as Map;
			stamp = new GameObject("TileStamp");
			stamp.hideFlags = HideFlags.HideAndDontSave;

			//Prevent a blank stamp on startup
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

		void OnPlayModeStateChanged(PlayModeStateChange stateChange)
		{
			switch (stateChange)
			{
				case PlayModeStateChange.ExitingEditMode:
                    //Unselect map to prevent dumb errors; Prevent "Some objects were not cleaned up when closing the scene" errors
                    if (stamp) DestroyImmediate(stamp);
					Selection.activeGameObject = null;

					//Auto connect all tiles
					ConnectAllTiles();
					break;
			}
		}

		#region Utility
		void ConnectAllTiles()
        {
            foreach (var t in m.tiles)
                AutoConnectNeighbourTiles(t, connectDiagonals, m.maxConnectHeightDifference);
        }
		
		/// <summary>
		/// Automatically connect tile to neighbouring tiles
		/// </summary>
		/// <param name="tolerance">Y zeroed connection tolerance</param>
		void AutoConnectNeighbourTiles(Tile subject, bool connectDiagonals = false, float maxHeightDifference = 0.5f, float tolerance = 0.1f)
		{
			//Find tiles within range (ie. within the distance of the map's tilesize)
			const float adjDist = 1f , diagDist = 1.414213f;

			//Get y zeroed position
			var sFlatPos = subject.transform.position; sFlatPos.y = 0;

			foreach (var t in m.tiles)
			{
				//Calculate vertical difference
				var heightDiff = Mathf.Abs(subject.transform.position.y - t.transform.position.y);

				//Get y zeroed position
				var tFlatPos = t.transform.position; tFlatPos.y = 0;

				//Calculate y zeroed distance
				float dist = Vector3.Distance(sFlatPos, tFlatPos);

				//Adjacent
				if ((dist - (adjDist * m.tileSize - tolerance)) * ((adjDist * m.tileSize + tolerance) - dist) >= 0)
				{
					//Only connect if below allowable height difference
					if (heightDiff > maxHeightDifference) return;

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
						if (heightDiff > maxHeightDifference) return;

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