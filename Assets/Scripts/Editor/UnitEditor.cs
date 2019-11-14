using System;
using StormRend.CameraSystem;
using StormRend.MapSystems;
using StormRend.MapSystems.Tiles;
using StormRend.Units;
using UnityEditor;
using UnityEngine;

namespace StormRend.Editors
{
	[CustomEditor(typeof(Unit), true)]
	[CanEditMultipleObjects]
	public class UnitEditor : SmartEditor
	{
		Unit u;     //Target
		Map map;
		Color buttonColour = new Color(1, 0.75f, 0);
		public override string[] propertiesToExclude => new[] { "m_Script" };

		void OnEnable()
		{
			u = target as Unit;
			map = Map.current;
		}

		void OnSceneGUI()
		{
			var e = Event.current;
			DrawRotateAndSnapButtons(buttonColour);
			switch (e.type)
			{
				case EventType.KeyDown:
					MoveByWASD(e);
					break;
				case EventType.MouseUp:
					SnapToNearestTile();
					break;
			}
		}

        void MoveByWASD(Event e)
        {
			var au = u as AnimateUnit;
			if (e.type == EventType.KeyDown)
				switch (e.keyCode)
				{
					case KeyCode.UpArrow:
						au.Push(new Vector2Int((int)au.transform.forward.x, (int)au.transform.forward.z), false);
						e.Use();
						break;
					case KeyCode.LeftArrow:
						u.transform.Rotate(Vector3.up, -90);
						e.Use();
						break;
					case KeyCode.DownArrow:
						au.Push(new Vector2Int((int)-au.transform.forward.x, (int)-au.transform.forward.z), false);
						e.Use();
						break;
					case KeyCode.RightArrow:
						u.transform.Rotate(Vector3.up, 90);
						e.Use();
						break;
				}
        }

        void DrawRotateAndSnapButtons(Color color)
		{
			Vector2 offset = new Vector2(18, 0);
			const float bSize = 20;
			var oldColor = GUI.color;
			GUI.color = color;
			var cam = SceneView.lastActiveSceneView.camera;
			var scene = cam.pixelRect;
			var w2s = cam.WorldToScreenPoint(u.transform.position);
			var leftButton = new Rect(w2s.x - offset.x - bSize, scene.height - w2s.y - offset.y, bSize, bSize);
			var snapButton = new Rect(w2s.x - bSize * 0.5f, scene.height - w2s.y, bSize, bSize);
			var rightbutton = new Rect(w2s.x + offset.x, scene.height - w2s.y - offset.y, bSize, bSize);
			Handles.BeginGUI();
			if (GUI.Button(leftButton, "←")) u.transform.Rotate(Vector3.up, -90);
			if (GUI.Button(snapButton, "▼")) SnapToNearestTile();
			if (GUI.Button(rightbutton, "→")) u.transform.Rotate(Vector3.up, 90);
			Handles.EndGUI();
			GUI.color = oldColor;
		}

		void SnapToNearestTile()
		{
			if (map.TryGetNearestTile(u.transform.position, out Tile nearest))
			{
				u.transform.position = nearest.transform.position;
				u.currentTile = nearest;
			}
		}
	}
}