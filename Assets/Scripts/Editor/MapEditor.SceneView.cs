using StormRend.Systems.Mapping;
using UnityEditor;
using UnityEngine;

namespace StormRend.Editors
{
	public partial class MapEditor : Editor
	{
		const int kNumOfGridLines = 20;
		Color oldHandleColor;

		void OnSceneGUI()
		{
			e = Event.current;
			oldHandleColor = Handles.color;

			SceneView.RepaintAll();
			// if (!t || !t.selectedTilePrefab) return;

			DrawStamp();
			DrawGrid(new Color(1f, 0.5f, 0));

			TestWorldCursor();

			Handles.color = oldHandleColor;
		}

		void DrawStamp()
		{
			//Snap to grid

		}

		void DrawGrid(Color color)
		{
			Handles.color = color;
			var lineLength = kNumOfGridLines * t.tileScaleXZ * 0.5f;
			//X lines
			for (int i = -kNumOfGridLines/2; i <= kNumOfGridLines/2; i++)
			{
				var start = new Vector3(t.tileScaleXZ * i, 0, -lineLength) + t.transform.position;
				var end = new Vector3(t.tileScaleXZ * i, 0, lineLength) + t.transform.position;
				Handles.DrawLine(start, end);
			}
			//Z lines
			for (int i = -kNumOfGridLines/2; i <= kNumOfGridLines/2; i++)
			{
				var start = new Vector3(-lineLength, 0 ,t.tileScaleXZ * i) + t.transform.position;
				var end = new Vector3(lineLength, 0, t.tileScaleXZ * i) + t.transform.position;
				Handles.DrawLine(start, end);
			}
		}


		void TestWorldCursor()
		{
			var ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);

			if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, t.layerMask))
			{
				Handles.color = Color.green;

				//Snap to the 
				worldCursor.x = Mathf.FloorToInt(hit.point.x / t.tileScaleXZ) * t.tileScaleXZ + t.tileScaleXZ * 0.5f;
				worldCursor.z = Mathf.FloorToInt(hit.point.z / t.tileScaleXZ) * t.tileScaleXZ + t.tileScaleXZ * 0.5f;
				Handles.RectangleHandleCap(1, worldCursor, Quaternion.AngleAxis(90, Vector3.right), t.tileScaleXZ * 0.5f, EventType.Repaint);
			}
		}


	}
}