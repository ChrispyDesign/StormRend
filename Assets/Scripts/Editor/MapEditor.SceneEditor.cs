using System;
using System.Collections.Generic;
using StormRend.Systems.Mapping;
using UnityEditor;
using UnityEngine;

namespace StormRend.Editors
{
    //------------- Scene Editor --------------
    public partial class MapEditor : Editor
    {
        public enum EditMode
            { Painting, Erasing }

        // List<Tile> eraseList = new List<Tile>();

        const int kNumOfGridLines = 30;
        public override bool RequiresConstantRepaint() => true;

        Color oldHandleColor, oldGUIColor;
        EditMode editMode;

        int controlID;
        private bool isEditing;

        #region Core
        void OnSceneGUIBegin()
        {
            oldHandleColor = Handles.color;
            oldGUIColor = GUI.color;

            //Events
            e = Event.current;

            //Hijack focus
            controlID = GUIUtility.GetControlID(FocusType.Passive);

            SceneView.RepaintAll();
        }
        void OnSceneGUI()
        {
            OnSceneGUIBegin();

            DrawGrid(new Color(1f, 0.5f, 0));
            DrawGridCursor();
            if (!t || !t.selectedTilePrefab) return;
            DrawStamp(gridCursor);

            HandleEvents();
            DrawTileTypeOverlayColour();

            OnSceneGUIEnd();
        }
        void OnSceneGUIEnd()
        {
            //Reset colors
            Handles.color = oldHandleColor;
            GUI.color = oldGUIColor;
        }
        #endregion

        #region Event Handling
        void HandleEvents()
        {
            editMode = (e.control || e.command) ? EditMode.Erasing : EditMode.Painting;
            switch (e.type)
            {
                case EventType.MouseDown:
                    HandleMouseDownEvents();
                    break;
                case EventType.MouseUp:
                    isEditing = false;  //Stop editing
                    break;
            }
            ContinueEditing();

            void HandleMouseDownEvents()
            {
                if (!e.alt)     //Let the user orbit
                {
                    switch (e.button)
                    {
                        case 0: //Left mouse button
                            isEditing = true;
                            PerformEdit();
                            GUIUtility.hotControl = controlID;  //Prevent unselect
                            e.Use();
                            break;
                    }
                }
            }

            void ContinueEditing()
            {
                if (isEditing)
                {
                    PerformEdit();
                }
            }
        }
        #endregion  //Event Handling

        #region Draw
        void DrawStamp(Vector3 center)
        {
            stamp.transform.position = center;
        }
		void DrawGrid(Color color, float alpha = 0.9f)
		{
			var dottedLineSize = 2f;

			Handles.color = new Color(color.r, color.g, color.b, alpha);
			var lineLength = kNumOfGridLines * t.tileSize * 0.5f;
			//Z lines
			for (int i = -kNumOfGridLines / 2; i <= kNumOfGridLines / 2; i++)
			{
				var start = new Vector3(t.tileSize * i, 0, -lineLength) + t.transform.position;
				var end = new Vector3(t.tileSize * i, 0, lineLength) + t.transform.position;
				Handles.DrawDottedLine(start, end, dottedLineSize);
			}
			//X lines
			for (int i = -kNumOfGridLines / 2; i <= kNumOfGridLines / 2; i++)
			{
				var start = new Vector3(-lineLength, 0, t.tileSize * i) + t.transform.position;
				var end = new Vector3(lineLength, 0, t.tileSize * i) + t.transform.position;
				Handles.DrawDottedLine(start, end, dottedLineSize);
			}
		}
		void DrawGridCursor(Color? color = null)
		{
			if (color == null) color = Color.white;
			if (editMode == EditMode.Erasing) color = Color.red;

			var ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);

			if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, 1 << t.gameObject.layer))      //The hidden editor raycast plane will always be on the same layer as the map object
			{
				Handles.color = color.Value;

				//Snap to the nearest grid square
				Vector3 offsetHit = hit.point - t.transform.position;
				Vector3 floor = new Vector3(Mathf.FloorToInt(offsetHit.x / t.tileSize), 0, Mathf.FloorToInt(offsetHit.z / t.tileSize));
				Vector3 offset = t.transform.position;
				Vector3 centre = new Vector3(t.tileSize * 0.5f, 0, t.tileSize * 0.5f);

				gridCursor = floor * t.tileSize + offset + centre;

				//Draw grid cursor
				Handles.RectangleHandleCap(1, gridCursor, Quaternion.AngleAxis(90, Vector3.right), t.tileSize * 0.5f, EventType.Repaint);
				// Handles.DrawSphere(2, snappedCursor, Quaternion.identity, t.tileSize * 0.25f);
			}
		}
		void DrawTileTypeOverlayColour()
		{

		}
        #endregion

		#region Edit
		void CreateStamp()
		{
			//Kill all children
			while (stamp.transform.childCount > 0)
				DestroyImmediate(stamp.transform.GetChild(0).gameObject);

			//Recreate at cursor position
			var go = Instantiate(t.selectedTilePrefab, gridCursor, Quaternion.identity);
			go.transform.SetParent(stamp.transform);
		}
		void PerformEdit()
		{
			if (editMode == EditMode.Painting)
				PerformStamp();
			else
				PerformErase();
		}
		void PerformStamp()
		{
			//Make sure there are no tiles in the current position
			if (IsOverTile(gridCursor, t.tileSize, out GameObject tileHit))
			{
				Debug.LogWarning("Can't stamp. There's a tile here already!");
				return;
			}

			//Instantiate a new tile prefab
			var newTile = Instantiate(t.selectedTilePrefab, gridCursor, Quaternion.identity);
			newTile.transform.SetParent(t.transform);
			newTile.gameObject.layer = t.gameObject.layer;

			//Register undo
			Undo.RegisterCreatedObjectUndo(newTile, "Stamp Tile Type" + t.selectedTilePrefab.name);

			//Add to map's list of tiles
			t.tiles.Add(newTile.GetComponent<Tile>());
		}
		void PerformErase()
		{
            if (IsOverTile(gridCursor, t.tileSize, out GameObject tileToErase))
            {
                //Erase the found tile
                t.tiles.Remove(tileToErase.GetComponent<Tile>());
                DestroyImmediate(tileToErase);
            }
		}
		#endregion

        #region Assists
		bool IsOverTile(Vector3 checkPos, float checkBoundsSize, out GameObject intersectedTile)
        {
            //BRUTE FORCE; Probably not very efficient
            float boundsFactor = 0.95f;
            var cursorBoundsSize = new Vector3(checkBoundsSize * boundsFactor, float.MaxValue, checkBoundsSize * boundsFactor);
            var cursorBounds = new Bounds(gridCursor, cursorBoundsSize);

            for (int i = 0; i < t.transform.childCount; ++i)
            {
                var child = t.transform.GetChild(i);

                //Check child is overlapping
                var bounds = child.GetComponentInChildren<Renderer>().bounds;

                //Tile found
                if (cursorBounds.Intersects(bounds))
                {
                    intersectedTile = child.gameObject;
                    return true;
                }
            }
            intersectedTile = null;
            return false;
        }
        #endregion
    }
}