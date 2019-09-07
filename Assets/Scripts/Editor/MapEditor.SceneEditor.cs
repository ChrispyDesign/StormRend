using System;
using System.Collections.Generic;
using StormRend.Systems.Mapping;
using UnityEditor;
using UnityEngine;

namespace StormRend.Editors
{
    //------------- Scene Editor --------------
    public partial class MapEditor : SmartEditor
    {
        public enum EditMode { Painting, Erasing }
		const int kNumOfGridLines = 30;
        public override bool RequiresConstantRepaint() => true;
		GameObject stamp;
		Vector3 gridCursor;
        EditMode editMode;

        int controlID;
        bool isEditing;

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

            DrawG(new Color(1f, 0.5f, 0));
            DrawGC();
            if (!m || !m.selectedTilePrefab) return;
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
		void DrawG(Color color, float alpha = 0.9f)
		{
			var dottedLineSize = 2f;
			Handles.color = new Color(color.r, color.g, color.b, alpha);
			var lineLength = kNumOfGridLines * m.tileSize * 0.5f;
			//Z lines
			for (int i = -kNumOfGridLines / 2; i <= kNumOfGridLines / 2; i++)
			{
				var start = new Vector3(m.tileSize * i, 0, -lineLength) + m.transform.position;
				var end = new Vector3(m.tileSize * i, 0, lineLength) + m.transform.position;
				Handles.DrawDottedLine(start, end, dottedLineSize);
			}
			//X lines
			for (int i = -kNumOfGridLines / 2; i <= kNumOfGridLines / 2; i++)
			{
				var start = new Vector3(-lineLength, 0, m.tileSize * i) + m.transform.position;
				var end = new Vector3(lineLength, 0, m.tileSize * i) + m.transform.position;
				Handles.DrawDottedLine(start, end, dottedLineSize);
			}
		}
		void DrawGC(Color? color = null)
		{
			if (color == null) color = Color.white;
			if (editMode == EditMode.Erasing) color = Color.red;
			var ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
			if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, 1 << m.gameObject.layer))      //The hidden editor raycast plane should always be on the same layer as the map object
			{
				Handles.color = color.Value;
				Vector3 oshit = hit.point - m.transform.position;
				Vector3 f = new Vector3(Mathf.FloorToInt(oshit.x / m.tileSize), 0, Mathf.FloorToInt(oshit.z / m.tileSize));
				Vector3 os = m.transform.position;
				Vector3 c = new Vector3(m.tileSize * 0.5f, 0, m.tileSize * 0.5f);
				gridCursor = f * m.tileSize + os + c;
				Handles.RectangleHandleCap(1, gridCursor, Quaternion.AngleAxis(90, Vector3.right), m.tileSize * 0.5f, EventType.Repaint);
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
			var go = Instantiate(m.selectedTilePrefab, gridCursor, Quaternion.identity);
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
			if (IsOverTile(gridCursor, m.tileSize, out GameObject tileHit))
			{
				Debug.LogWarning("Cannot paint on existing tile!");
				return;
			}

			//Instantiate a new tile prefab
            var rotation = randomizePaintDirection ? Quaternion.AngleAxis(90 * UnityEngine.Random.Range(0, 4), Vector3.up) : Quaternion.identity;
			var newTile = Instantiate(m.selectedTilePrefab, gridCursor, rotation);
			newTile.transform.SetParent(m.transform);
			newTile.gameObject.layer = m.gameObject.layer;

			Undo.RegisterCreatedObjectUndo(newTile, "Paint Tile " + m.selectedTilePrefab.name);

			//Add to map's list of tiles
			m.tiles.Add(newTile.GetComponent<Tile>());
		}
		void PerformErase()
		{
            if (IsOverTile(gridCursor, m.tileSize, out GameObject tileToErase))
            {
                //Erase the found tile
                m.tiles.Remove(tileToErase.GetComponent<Tile>());

                Undo.DestroyObjectImmediate(tileToErase);
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

            for (int i = 0; i < m.transform.childCount; ++i)
            {
                var child = m.transform.GetChild(i);

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