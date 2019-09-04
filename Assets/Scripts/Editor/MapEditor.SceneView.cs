using StormRend.Systems.Mapping;
using UnityEditor;
using UnityEngine;

namespace StormRend.Editors
{
	public partial class MapEditor : Editor
	{
		const int kNumOfGridLines = 30;
		Color oldHandleColor;

		void OnSceneGUI()
		{
			e = Event.current;
			oldHandleColor = Handles.color;

			SceneView.RepaintAll();
			// if (!t || !t.selectedTilePrefab) return;
			DrawGrid(new Color(1f, 0.5f, 0));
			DrawSnappedCursor();
			UpdateStamp();

			Handles.color = oldHandleColor;
		}

		void UpdateStamp()
		{
			//Snap to grid
		}

		void DrawGrid(Color color, float alpha = 0.9f)
		{
			var dottedLineSize = 2f;

			Handles.color = new Color(color.r, color.g, color.b, alpha);
			var lineLength = kNumOfGridLines * t.tileSize * 0.5f;
			//Z lines
			for (int i = -kNumOfGridLines/2; i <= kNumOfGridLines/2; i++)
			{
				var start = new Vector3(t.tileSize * i, 0, -lineLength) + t.transform.position;
				var end = new Vector3(t.tileSize * i, 0, lineLength) + t.transform.position;
				Handles.DrawDottedLine(start, end, dottedLineSize);
			}
			//X lines
			for (int i = -kNumOfGridLines/2; i <= kNumOfGridLines/2; i++)
			{
				var start = new Vector3(-lineLength, 0 ,t.tileSize * i) + t.transform.position;
				var end = new Vector3(lineLength, 0, t.tileSize * i) + t.transform.position;
				Handles.DrawDottedLine(start, end, dottedLineSize);
			}
		}

		void DrawSnappedCursor(Color? color = null)
		{
			if (color == null) color = Color.white;

			var ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);

			if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, 1 << t.gameObject.layer))		//The hidden editor raycast plane will always be on the same layer as the map object
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