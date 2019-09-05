using StormRend.Tools;
using UnityEditor;
using UnityEngine;

namespace StormRend.Editors
{
	public partial class LandscaperEditor : Editor
	{
		void OnSceneGUI()
		{
			SceneView.RepaintAll();
			if (t == null || t.SelectedPrefab == null) return;
			var isErasing = Event.current.control;
			var controlId = GUIUtility.GetControlID(FocusType.Passive);
			var mousePos = Event.current.mousePosition;

			var ray = HandleUtility.GUIPointToWorldRay(mousePos);

			if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, t.layerMask))
			{
				worldCursor = hit.point;
				var up = t.followOnSurface ? hit.normal : Vector3.up;
				Handles.color = isErasing ? Color.red : Color.white;
				Handles.DrawWireDisc(worldCursor, up, t.brushRadius);
				Handles.color = Color.white * 0.5f;
				Handles.DrawWireDisc(worldCursor + up * t.brushHeight, up, t.brushRadius);
				Handles.DrawWireDisc(worldCursor - up * t.brushHeight, up, t.brushRadius);
				OverlapCapsule(worldCursor + hit.normal * 10, worldCursor - hit.normal * 10, t.brushRadius, t.layerMask);
				if (isErasing)
					DrawEraser(worldCursor, hit.normal);
				else
					DrawStamp(worldCursor, hit.normal);
			}

			switch (Event.current.type)
			{
				case EventType.ScrollWheel:
					if (Event.current.shift)
					{
						RotateStamp(Event.current.delta);
						Event.current.Use();
					}
					if (Event.current.alt)
					{
						t.brushRadius *= Event.current.delta.y < 0 ? 0.9f : 1.1f;
						CreateNewStamp();
						Event.current.Use();
					}
					break;
				case EventType.KeyDown:
					HandleKey(Event.current.keyCode);
					break;
				case EventType.MouseDown:
					//If not using the default orbit mode...
					if (Event.current.button == 0 && !Event.current.alt)
					{
						if (isErasing)
							PerformErase();
						else
							PerformStamp();
						GUIUtility.hotControl = controlId;
						Event.current.Use();
					}
					break;
			}
		}

		private void OverlapCapsule(Vector3 top, Vector3 bottom, float brushRadius, LayerMask layerMask)
		{
			overlaps.Clear();
			overlappedGameObjects.Clear();
			if (t.collisionTest == Landscaper.CollisionTest.ColliderBounds)
			{
				foreach (var c in Physics.OverlapCapsule(top, bottom, brushRadius))
				{
					if (c.transform.parent == t.rootTransform)
					{
						overlaps.Add(c.bounds);
						overlappedGameObjects.Add(c.gameObject);
					}
				}
			}
			if (t.collisionTest == Landscaper.CollisionTest.RendererBounds)
			{
				//TODO: This might need an oct-tree later. Brute force for now.
				var capsule = new Bounds(Vector3.Lerp(top, bottom, 0.5f), new Vector3(brushRadius * 2, brushRadius * 2 + (top - bottom).magnitude, brushRadius * 2));
				for (var i = 0; i < t.rootTransform.childCount; i++)
				{
					var child = t.rootTransform.GetChild(i);
					var bounds = child.GetComponentInChildren<Renderer>().bounds;
					if (capsule.Intersects(bounds))
					{
						overlaps.Add(bounds);
						overlappedGameObjects.Add(child.gameObject);
					}
				}
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
					t.brushDensity *= 0.9f;
					CreateNewStamp();
					Event.current.Use();
					break;
				case KeyCode.Equals:
					t.brushDensity *= 1.1f;
					CreateNewStamp();
					Event.current.Use();
					break;

				case KeyCode.Space:
					CreateNewStamp();
					Event.current.Use();
					break;
				case KeyCode.LeftBracket:
					t.brushRadius *= 0.9f;
					CreateNewStamp();
					Event.current.Use();
					break;
				case KeyCode.RightBracket:
					t.brushRadius *= 1.1f;
					CreateNewStamp();
					Event.current.Use();
					break;

			}
		}

		void DrawStamp(Vector3 center, Vector3 normal)
		{
			stamp.transform.position = center;

			if (t.followOnSurface)
			{
				var tangent = Vector3.Cross(normal, Vector3.forward);
				if (tangent.magnitude == 0)
					tangent = Vector3.Cross(normal, Vector3.up);
				if (normal.magnitude < Mathf.Epsilon || tangent.magnitude < Mathf.Epsilon) return;
				stamp.transform.rotation = Quaternion.LookRotation(tangent, normal);
			}
			else
			{
				stamp.transform.rotation = Quaternion.identity;
			}

			for (var i = 0; i < stamp.transform.childCount; i++)
			{
				var child = stamp.transform.GetChild(i);
				child.localPosition = Vector3.Scale(child.localPosition, new Vector3(1, 0, 1));
				RaycastHit hit;
				if (Physics.Raycast(child.position + (child.up * t.brushHeight), -child.up, out hit, t.brushHeight * 2, t.layerMask))
				{
					var slope = Vector3.Angle(normal, hit.normal);
					if (slope > t.maxSlope)
					{
						child.gameObject.SetActive(false);
						continue;
					}
					child.gameObject.SetActive(true);
					var dummy = child.GetChild(0);
					dummy.position = hit.point;
					if (t.alignToNormal)
					{
						var tangent = Vector3.Cross(hit.normal, child.forward);
						if (tangent.magnitude == 0)
							tangent = Vector3.Cross(hit.normal, child.up);
						dummy.rotation = Quaternion.LookRotation(tangent, hit.normal);
					}
					else
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
							if (maxIntersection > t.maxIntersectionVolume)
							{
								child.gameObject.SetActive(false);
							}
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

			stamp.transform.position = center;
			if (t.followOnSurface)
			{
				var tangent = Vector3.Cross(normal, Vector3.forward);
				if (tangent.magnitude == 0)
					tangent = Vector3.Cross(normal, Vector3.up);
				if (normal.magnitude < Mathf.Epsilon || tangent.magnitude < Mathf.Epsilon) return;
				stamp.transform.rotation = Quaternion.LookRotation(tangent, normal);
			}
			else
			{
				stamp.transform.rotation = Quaternion.identity;
			}

			for (var i = 0; i < overlaps.Count; i++)
			{
				var h = overlaps[i];
				Handles.color = Color.red;
				Handles.DrawWireDisc(h.center, Vector3.up, h.extents.magnitude);
				erase.Add(overlappedGameObjects[i]);
			}

		}

		public override bool RequiresConstantRepaint() { return true; }
	}
}