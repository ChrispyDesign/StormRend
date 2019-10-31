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
		Color buttonColour = new Color(1, 0.5f, 0);
		public override string[] propertiesToExclude => new[] { "m_Script" };

		void OnEnable()
		{
			u = target as Unit;
			map = Map.current;
		}

		void OnSceneGUI()
		{
			var e = Event.current;
			DrawRotateButtons(buttonColour);
			SnapToNearestTile(e);
			MoveByWASD(e);
		}

        void MoveByWASD(Event e)
        {
			var au = u as AnimateUnit;
			if (e.type == EventType.KeyDown)
				switch (e.keyCode)
				{
					case KeyCode.H:
						au.Push(new Vector2Int((int)au.transform.forward.x, (int)au.transform.forward.z), false);
						break;
					case KeyCode.B:
						u.transform.Rotate(Vector3.up, -90);
						break;
					case KeyCode.N:
						au.Push(new Vector2Int((int)-au.transform.forward.x, (int)-au.transform.forward.z), false);
						break;
					case KeyCode.M:
						u.transform.Rotate(Vector3.up, 90);
						break;
				}
        }

        void DrawRotateButtons(Color color)
		{
			Vector2 offset = new Vector2(10, 0);
			const float bSize = 30f;
			var oldColor = GUI.color;
			GUI.color = color;
			var cam = SceneView.lastActiveSceneView.camera;
			var scene = cam.pixelRect;
			var w2s = cam.WorldToScreenPoint(u.transform.position);
			var leftButton = new Rect(w2s.x - offset.x - bSize, scene.height - w2s.y - offset.y, bSize, bSize * 0.8f);
			var rightbutton = new Rect(w2s.x + offset.x, scene.height - w2s.y - offset.y, bSize, bSize * 0.8f);
			Handles.BeginGUI();
			if (GUI.Button(leftButton, "<")) u.transform.Rotate(Vector3.up, -90);
			if (GUI.Button(rightbutton, ">")) u.transform.Rotate(Vector3.up, 90);
			Handles.EndGUI();
			GUI.color = oldColor;
		}

		void SnapToNearestTile(Event e)
		{
			if (e.type == EventType.MouseUp)
				if (map.TryGetNearestTile(u.transform.position, out Tile nearest))
				{
					u.transform.position = nearest.transform.position;
					u.currentTile = nearest;
				}
		}
	}
}