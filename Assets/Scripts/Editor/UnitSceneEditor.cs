// using StormRend.CameraSystem;
// using StormRend.MapSystems;
// using StormRend.MapSystems.Tiles;
// using StormRend.Units;
// using UnityEditor;
// using UnityEngine;

// namespace StormRend.Editors
// {
// 	[CustomEditor(typeof(AllyUnit))]
// 	public class UnitSceneEditor : Editor
// 	{
// 		Unit u;		//Target
// 		Map map;
// 		float tileSize = 2;
// 		LayerMask layer;
// 		Tile interimTile;

// 		void OnEnable()
// 		{
// 			u = target as Unit;
// 			map = Map.current;
// 			tileSize = map.tileSize;
// 		}

// 		void OnSceneGUI()
// 		{
// 			// ProcessEvents();

// 			Ray ray = MasterCamera.current.camera.ScreenPointToRay(Input.mousePosition);

// 			Physics.Raycast(ray, out RaycastHit hitInfo, 5000f, LayerMask.NameToLayer("Level"));
// 			interimTile = hitInfo.collider.GetComponent<Tile>();

// 			//Put an object where tile is
// 			if (interimTile)
// 				Handles.DrawSphere(1, interimTile.transform.position, interimTile.transform.rotation, 1f);
// 				// Handles.SphereHandleCap(2, interimTile.transform.position, interimTile.transform.rotation, 1f);

// 		}

// 		void ProcessEvents()
// 		{
// 			var e = Event.current;

// 			switch (e.type)
// 			{
// 				case EventType.MouseUp:
// 					//Move unit to the nearest tile
// 					if (map.TryGetNearestTile(u.transform.position, out Tile nearest))
// 					{
// 						u.transform.position = nearest.transform.position;
// 						u.currentTile = nearest;
// 					}
// 					break;
// 			}
			
// 		}
// 	}
// }