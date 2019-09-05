using System;
using StormRend.Systems.Mapping;
using UnityEditor;
using UnityEngine;

namespace StormRend.Editors
{
	//------------- Scene Editor --------------
    public partial class MapEditor : Editor
    {
		public enum PaintMode
		{
			Painting, Erasing
		}

        const int kNumOfGridLines = 30;
        Color oldHandleColor;
        Color oldGUIColor;
		PaintMode mode;
        private int controlID;

        #region Core
        void OnSceneGUIBegin()
        {
            oldHandleColor = Handles.color;
            oldGUIColor = GUI.color;

            //Events
            e = Event.current;

            //Hijack focus
            controlID = GUIUtility.GetControlID(FocusType.Passive);

			//Is in erasing mode?
           	mode = e.control ? PaintMode.Erasing : PaintMode.Painting;

            SceneView.RepaintAll();
        }
        void OnSceneGUI()
        {
            OnSceneGUIBegin();

            DrawGrid(new Color(1f, 0.5f, 0));
            DrawSnappedCursor();
            if (!t || !t.selectedTilePrefab) return;
            DrawStamp(snappedCursor);

            HandleEvents();
            DrawTileTypeOverlayColour();

            OnSceneGUIEnd();
        }

        void DrawStamp(Vector3 center)
        {
            stamp.transform.position = center;
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
            switch (e.type)
            {
                case EventType.KeyDown:
                    HandleKeyEvents(e);
                    break;
                case EventType.MouseMove:
                    HandleMouseMoveEvents(e);
                    break;
                case EventType.MouseDown:
                    HandleMouseEvents(e);
                    break;
            }
        }
        void HandleKeyEvents(Event e)
        {
            // //Erase
            // if (e.control || e.command) //Both sides
            // {
            // 	isErasing = true;
            // 	return;
            // }
            // else if (e.alt) //Both sides
            // {
            // }
        }
        void HandleMouseMoveEvents(Event e)
        {
            if (!e.alt)     //Let the user orbit
            {
                //Handle continuous painting?
            }
        }
        void HandleMouseEvents(Event e)
        {
            if (!e.alt)     //Let the user orbit
            {
                switch (e.button)
                {
                    case 0: //Left mouse button
                        if (mode == PaintMode.Erasing)
                            PeformErase();
                        else
                            PeformStamp();
                        GUIUtility.hotControl = controlID;
                        e.Use();
                        break;
                    // case 1: //Right mouse button
                    //     Debug.Log("Right click!");
                    //     break;
                    case 2: //Middle mouse button
                        Debug.Log("Middle click!");
                        e.Use();
                        break;
                }
            }
        }
        #endregion  //Event Handling

        void DrawTileTypeOverlayColour()
        {

        }

        void CreateStamp()
        {
            //Kill all children
            while (stamp.transform.childCount > 0)
                DestroyImmediate(stamp.transform.GetChild(0).gameObject);

            //Recreate at cursor position
            var go = Instantiate(t.selectedTilePrefab, snappedCursor, Quaternion.identity);
            go.transform.SetParent(stamp.transform);
        }

        private void PeformErase()
        {
            Debug.Log("Erase!");
        }

        private void PeformStamp()
        {
            Debug.Log("Stamp!");

            //Make sure there are no tiles in the current position
            var halfExtents = new Vector3(t.tileSize * 0.5f * 0.9f, float.MaxValue, t.tileSize * 0.5f * 0.9f);
            if (Physics.BoxCast(snappedCursor, halfExtents, Vector3.up, Quaternion.identity, float.MaxValue, 1 << t.gameObject.layer))
            {
                //Don't stamp if there's already a tile here!
                Debug.LogWarning("Tile already exists here!");
                return;
            }
            //Instantiate a new tile prefab
            var newTile = Instantiate(t.selectedTilePrefab, snappedCursor, Quaternion.identity);
            //parent to the root
            newTile.transform.SetParent(t.root);
            //Register undo
            Undo.RegisterCreatedObjectUndo(newTile, "Stamp Tile Type" + t.selectedTilePrefab.name);
            //Add to list of tiles
            t.tiles.Add(newTile.GetComponent<Tile>());
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

        void DrawSnappedCursor(Color? color = null)
        {
            if (color == null) color = Color.white;
            if (mode == PaintMode.Erasing) color = Color.red;

            var ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, 1 << t.gameObject.layer))      //The hidden editor raycast plane will always be on the same layer as the map object
            {
                Handles.color = color.Value;

                //Snap to the nearest grid square
                Vector3 offsetHit = hit.point - t.transform.position;
                Vector3 floor = new Vector3(Mathf.FloorToInt(offsetHit.x / t.tileSize), 0, Mathf.FloorToInt(offsetHit.z / t.tileSize));
                Vector3 offset = t.transform.position;
                Vector3 centre = new Vector3(t.tileSize * 0.5f, 0, t.tileSize * 0.5f);

                snappedCursor = floor * t.tileSize + offset + centre;

                //Draw grid cursor
                Handles.RectangleHandleCap(1, snappedCursor, Quaternion.AngleAxis(90, Vector3.right), t.tileSize * 0.5f, EventType.Repaint);
                // Handles.DrawSphere(2, snappedCursor, Quaternion.identity, t.tileSize * 0.25f);
            }
        }


    }
}