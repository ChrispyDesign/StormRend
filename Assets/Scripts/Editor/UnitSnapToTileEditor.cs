using StormRend.CameraSystem;
using StormRend.MapSystems;
using StormRend.MapSystems.Tiles;
using StormRend.Units;
using UnityEditor;
using UnityEngine;

namespace StormRend.Editors
{
	[CustomEditor(typeof(Unit), true)]
	public class UnitSnapToTileEditor : SmartEditor
	{
		Unit u;     //Target
		Map map;

		public override string[] propertiesToExclude => new[] { "m_Script" };

		void OnEnable()
		{
			u = target as Unit;
			map = Map.current;
		}

		void OnSceneGUI()
		{
			//On mouse up snap to the nearest tile
			if (Event.current.type == EventType.MouseUp)
				if (map.TryGetNearestTile(u.transform.position, out Tile nearest))
				{
					u.transform.position = nearest.transform.position;
					u.currentTile = nearest;
				}

		}
	}
}