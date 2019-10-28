using StormRend.CameraSystem;
using StormRend.MapSystems;
using StormRend.MapSystems.Tiles;
using StormRend.Units;
using UnityEditor;
using UnityEngine;

namespace StormRend.Editors
{
	[CustomEditor(typeof(Unit), true)]
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
			DrawRotateButtons(buttonColour);
			SnapToNearestTile();
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

		void SnapToNearestTile()
		{
			if (Event.current.type == EventType.MouseUp)
				if (map.TryGetNearestTile(u.transform.position, out Tile nearest))
				{
					u.transform.position = nearest.transform.position;
					u.currentTile = nearest;
				}
		}
	}
}