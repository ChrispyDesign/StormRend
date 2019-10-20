using StormRend.MapSystems.Tiles;
using UnityEditor;
using UnityEngine;

namespace StormRend.Editors
{
	//------------- Scene Editor --------------
	public partial class MapEditor : SmartEditor
    {
        public enum EditMode { Painting, Erasing }

        const int kNumOfGridLines = 40;		//Size of the grid
        public override bool RequiresConstantRepaint() => true;

        Color oldHandleColor, oldGUIColor;
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

            DrawGrid(new Color(1f, 0.5f, 0), 0.6f, 1f);
            SetMouseCursor();
            DrawGridCursor();

            if (showConnections) DrawConnections(Color.red);

            if (!m || !m.selectedTilePrefab) return;

            DrawStamp(gridCursor);

            HandleEvents();

            OnSceneGUIEnd();
        }

        void SetMouseCursor()
        {
            switch (editMode)
            {
                case EditMode.Painting:
                    EditorGUIUtility.AddCursorRect(new Rect(0, 0, 10000, 10000), MouseCursor.ArrowPlus);
                    break;
                case EditMode.Erasing:
                    EditorGUIUtility.AddCursorRect(new Rect(0, 0, 10000, 10000), MouseCursor.ArrowMinus);
                    break;
            }
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
            //Position
            stamp.transform.position = center;

            //Visibility
            stamp.SetActive((editMode == EditMode.Painting) ? true : false);
        }
        void DrawGrid(Color color, float alpha = 0.9f, float dottedLineSize = 2f)
        {
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
        void DrawGridCursor(Color? color = null)
        {
            if (color == null) color = Color.white;
            if (editMode == EditMode.Erasing) color = Color.red;

            var ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, 1 << m.gameObject.layer))      //The hidden editor raycast plane will always be on the same layer as the map object
            {
                Handles.color = color.Value;

                //Snap to the nearest grid square
                Vector3 offsetHit = hit.point - m.transform.position;
                Vector3 floor = new Vector3(Mathf.FloorToInt(offsetHit.x / m.tileSize), 0, Mathf.FloorToInt(offsetHit.z / m.tileSize));
                Vector3 offset = m.transform.position;
                Vector3 centre = new Vector3(m.tileSize * 0.5f, 0, m.tileSize * 0.5f);

                gridCursor = floor * m.tileSize + offset + centre;

                //Draw grid cursor
                Handles.RectangleHandleCap(0, gridCursor, Quaternion.AngleAxis(90, Vector3.right), m.tileSize * 0.499f, EventType.Repaint);
                Handles.RectangleHandleCap(1, gridCursor, Quaternion.AngleAxis(90, Vector3.right), m.tileSize * 0.5f, EventType.Repaint);
                Handles.RectangleHandleCap(2, gridCursor, Quaternion.AngleAxis(90, Vector3.right), m.tileSize * 0.501f, EventType.Repaint);
            }
        }
        void DrawConnections(Color? color = null)
        {
            Handles.color = color == null ? Color.white : color.Value;

            foreach (var t in m?.tiles)
            {
                Vector3 start = t.transform.position;

                foreach (var c in t?.connections)
                {
                    Vector3 end = c.transform.position;

                    Handles.DrawLine(start, end);

                    // Handles.DrawBezier(start, end, start + Vector3.up, end + Vector3.up, Color.red, null, 2);
                    // Handles.DrawSphere(0, start, Quaternion.identity, m.tileSize / 20f);
                    // Handles.color = new Color(0.6f, 1, 0.1f);
                }
            }
        }
	#endregion

	#region Edit
        void CreateStamp()
        {
            //Kill all children
            while (stamp.transform.childCount > 0)
                DestroyImmediate(stamp.transform.GetChild(0).gameObject);

            //Recreate at cursor position
            if (!m.selectedTilePrefab) return;  //null check
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
            if (IsOverTile(gridCursor, m.tileSize * 0.95f, out GameObject tileHit))
            {
                Debug.LogWarning("Cannot paint on existing tile!");
                return;
            }

			//This creates a new undo event for every object instantiated instead of grouping them in all the same operations
			// Undo.IncrementCurrentGroup();	

			//Instantiate a new tile prefab
			//According to current cursor positions and rotation settings
			var rotation = randomizePaintDirection ? Quaternion.AngleAxis(90 * UnityEngine.Random.Range(0, 4), Vector3.up) : Quaternion.identity;	//Rotation
			var t = PrefabUtility.InstantiatePrefab(m.selectedTilePrefab, m.transform) as Tile;
			t.transform.position = gridCursor;
			t.transform.rotation = rotation;
			t.gameObject.layer = m.gameObject.layer;
			t.owner = m;

			//UNDO WORKING
            Undo.RegisterCreatedObjectUndo(t.gameObject, "Paint Tile " + m.selectedTilePrefab.name);

            //Add to map's list of tiles
            m.tiles.Add(t);
        }
        void PerformErase()
        {
            if (IsOverTile(gridCursor, m.tileSize * 0.95f, out GameObject tileToErase))
            {
                //TODO Temporary-better-than-nothing-solution
                //Just clear all the connections to prevent null reference exceptions
                m.ClearAllTileConnections();

                //Erase the found tile
                m.tiles.Remove(tileToErase.GetComponent<Tile>());

				//UNDO WORKING
                Undo.DestroyObjectImmediate(tileToErase);
            }
        }
	#endregion

	#region Assists
        void OnUndoRedo()
        {
            //Clean up mess left behind from Undo system
            //Add tiles that aren't in the list
            for (int i = 0; i < m.transform.childCount; i++)
            {
                var childTile = m.transform.GetChild(i).GetComponent<Tile>();
                if (!m.tiles.Contains(childTile))
                    m.tiles.Add(childTile);
            }
            //Remove all tiles that are null
            m.tiles.RemoveAll(x => x == null);
        }

        /// <summary>
        /// Returns true if a position is over a tile. Outs the tile it is over
        /// </summary>
        bool IsOverTile(Vector3 checkPos, float checkBoundsSize, out GameObject intersectedTile)
        {
            //BRUTE FORCE; Probably not very efficient
            if (boundsType == BoundsType.RendererBounds)
            {
                var cursorBoundsSize = new Vector3(checkBoundsSize, float.MaxValue, checkBoundsSize);
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
            }
            else if (boundsType == BoundsType.ColliderBounds)
            {

            }
            intersectedTile = null;
            return false;
        }
	#endregion
    }
}