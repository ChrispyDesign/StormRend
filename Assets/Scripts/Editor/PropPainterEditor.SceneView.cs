using StormRend.MapSystems;
using UnityEditor;
using UnityEngine;

namespace StormRend.Editors
{
	public partial class PropPainterEditor : SmartEditor
	{
		const float cylinderCastSize = 1000f;

		void OnSceneGUI()
		{
			//Get current event
			var e = Event.current;

			SceneView.RepaintAll();
			if (pp == null || pp.SelectedPrefab == null) return;
			bool isErasing = e.control;
			var controlId = GUIUtility.GetControlID(FocusType.Passive);
			Vector2 mousePos = e.mousePosition;
			Ray ray = HandleUtility.GUIPointToWorldRay(mousePos);

			//Draw stamp if an appropriate surface detected
			if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, pp.layerMask))
			{
				worldCursor = hit.point;											//Position
				// Vector3 up = pp.followOnSurface ? hit.normal : Vector3.up;		//Normal
				Handles.color = isErasing ? Color.red : Color.white;				//Mode Color
				Handles.DrawWireDisc(worldCursor, Vector3.up, pp.brushRadius);		//Draw cursor

				//Draw stamp
				CalculateOverlappingObjects(worldCursor + Vector3.up * 1000f, worldCursor + Vector3.down * 1000f, pp.brushRadius);
				if (isErasing)
					DrawEraser(worldCursor, hit.normal);
				else
					DrawStamp(worldCursor, hit.normal);
			}

			switch (e.type)
			{
				case EventType.ScrollWheel:
					if (e.shift)
					{
						RotateStamp(e.delta);
						e.Use();
					}
					if (e.alt)
					{
						pp.brushRadius *= -e.delta.y < 0 ? 0.9f : 1.1f;
						CreateNewStamp();
						e.Use();
					}
					break;

				case EventType.KeyDown:
					HandleKey(e.keyCode);
					break;

				case EventType.MouseDown:
					//If not using the default orbit mode...
					if (e.button == 0 && !e.alt)
					{
						if (isErasing)
							PerformErase();
						else
							PerformStamp();
						GUIUtility.hotControl = controlId;
						e.Use();
					}
					break;
			}
		}


		/// <summary>
		/// Detects any objects that are overlapping
		/// </summary>
		void CalculateOverlappingObjects(Vector3 top, Vector3 bottom, float brushRadius)
		{
			overlaps.Clear();
			overlappedGameObjects.Clear();

			switch (pp.collisionTest)
			{
				case PropPainter.CollisionTest.ColliderBounds:
				{
					foreach (var c in Physics.OverlapCapsule(top, bottom, brushRadius))
					{
						//Make sure found object is in the root transform
						if (c.transform.parent == pp.rootTransform)
						{
							overlaps.Add(c.bounds);
							overlappedGameObjects.Add(c.gameObject);
						}
					}
				}
				break;

				case PropPainter.CollisionTest.RendererBounds:
				{
					//TODO: This might need an oct-tree later. Brute force for now.
					var overlapBounds = new Bounds(Vector3.Lerp(top, bottom, 0.5f), new Vector3(brushRadius * 2, (brushRadius * 2) + (top - bottom).magnitude, brushRadius * 2));
					for (var i = 0; i < pp.rootTransform.childCount; i++)
					{
						Transform child = pp.rootTransform.GetChild(i);

						var rendBounds = child.GetComponentInChildren<Renderer>().bounds;

						//Only add if the objects don't intersect
						if (overlapBounds.Intersects(rendBounds))
						{
							overlaps.Add(rendBounds);
							overlappedGameObjects.Add(child.gameObject);
						}
					}
				}
				break;
			}
		}

		void HandleKey(KeyCode keyCode)
		{
			switch (keyCode)
			{
				case KeyCode.Period:
					AdjustMaxScale(0.9f);
					break;
				case KeyCode.Slash:
					AdjustMaxScale(1.1f);
					break;
				case KeyCode.Minus:
					pp.brushDensity *= 0.9f;
					CreateNewStamp();
					Event.current.Use();
					break;
				case KeyCode.Equals:
					pp.brushDensity *= 1.1f;
					CreateNewStamp();
					Event.current.Use();
					break;

				case KeyCode.Space:
					CreateNewStamp();
					Event.current.Use();
					break;
				case KeyCode.LeftBracket:
					pp.brushRadius *= 0.9f;
					CreateNewStamp();
					Event.current.Use();
					break;
				case KeyCode.RightBracket:
					pp.brushRadius *= 1.1f;
					CreateNewStamp();
					Event.current.Use();
					break;

			}
		}

		void DrawStamp(Vector3 center, Vector3 normal)
		{
			//Position stamp
			stamp.transform.SetPositionAndRotation(center, Quaternion.identity);

			//Loop through all dummy objects in the stamp
			for (var i = 0; i < stamp.transform.childCount; i++)
			{
				//Get this dummy object
				var child = stamp.transform.GetChild(i);

				//Level child
				child.localPosition = Vector3.Scale(child.localPosition, new Vector3(1, 0, 1));

				//Do a cylinder cast, return true if anything hit
				//Raycast(origin, direction, raycasthit, maxDistance, layermask)
				if (Physics.Raycast(child.position + (child.up * cylinderCastSize), -child.up, out RaycastHit hit, cylinderCastSize * 2f, pp.layerMask))
				{
					child.gameObject.SetActive(true);
					var dummy = child.GetChild(0);
					dummy.position = hit.point;

					dummy.rotation = Quaternion.LookRotation(child.forward, child.up);

					var bounds = child.GetComponentInChildren<Renderer>().bounds;
					var childVolume = bounds.size.x * bounds.size.y * bounds.size.z;
					foreach (var b in overlaps)
					{

						if (b.Intersects(bounds))
						{
							var overlapVolume = b.size.x * b.size.y * b.size.z;
							var intersection = Intersection(b, bounds);
							var intersectionVolume = intersection.size.x * intersection.size.y * intersection.size.z;

							// Handles.DrawWireCube(intersection.center, intersection.size);
							var maxIntersection = Mathf.Max(intersectionVolume / overlapVolume, intersectionVolume / childVolume);

							// Handles.Label(intersection.center, maxIntersection.ToString());
							if (maxIntersection > pp.maxDensity)
								child.gameObject.SetActive(false);
						}
					}
				}
				else
				{
					child.gameObject.SetActive(false);
				}
			}
		}

		Bounds Intersection(Bounds A, Bounds B)
		{
			var min = new Vector3(Mathf.Max(A.min.x, B.min.x), Mathf.Max(A.min.y, B.min.y), Mathf.Max(A.min.z, B.min.z));
			var max = new Vector3(Mathf.Min(A.max.x, B.max.x), Mathf.Min(A.max.y, B.max.y), Mathf.Min(A.max.z, B.max.z));
			return new Bounds(Vector3.Lerp(min, max, 0.5f), max - min);
		}

		void DrawEraser(Vector3 center, Vector3 normal)
		{
			erase.Clear();
			for (var i = 0; i < stamp.transform.childCount; i++)
				stamp.transform.GetChild(i).gameObject.SetActive(false);

			stamp.transform.SetPositionAndRotation(center, Quaternion.identity);

			for (var i = 0; i < overlaps.Count; i++)
			{
				var h = overlaps[i];
				Handles.color = Color.red;
				Handles.DrawWireDisc(h.center, Vector3.up, h.extents.magnitude);
				erase.Add(overlappedGameObjects[i]);
			}

		}

		public override bool RequiresConstantRepaint() => true;
	}
}