using System.Collections.Generic;
using StormRend.Tools;
using UnityEditor;
using UnityEngine;

namespace StormRend.Editors
{
	[CustomEditor(typeof(Landscaper))]
	public partial class LandscaperEditor : Editor
	{
		Texture2D[] paletteImages;
		GameObject stamp;

		List<GameObject> erase = new List<GameObject>();
		Vector3 worldCursor;
		Landscaper gardener;
		List<Bounds> overlaps = new List<Bounds>();
		List<GameObject> overlappedGameObjects = new List<GameObject>();

		void OnEnable()
		{
			stamp = new GameObject("Stamp");
			stamp.hideFlags = HideFlags.HideAndDontSave;
			gardener = target as Landscaper;
			
			// if (ip.SelectedPrefab != null)
			// {
			// 	variations = ip.SelectedPrefab.GetComponent<Variations>();
			// 	if (variationsEditor != null)
			// 		DestroyImmediate(variationsEditor);
			// 	if (variations != null)
			// 		variationsEditor = Editor.CreateEditor(variations);
			// 	CreateNewStamp();
			// }

		}

		void OnDisable()
		{
			if (stamp != null)
				DestroyImmediate(stamp);
		}

		void CreateNewStamp()
		{
			//Destroy stamp
			while (stamp.transform.childCount > 0)
				DestroyImmediate(stamp.transform.GetChild(0).gameObject);

			//Determine number of objects in stamp
			var count = Mathf.Min(1000, (Mathf.PI * Mathf.Pow(gardener.brushRadius, 2)) / (1f / gardener.brushDensity));


			for (var i = 0; i < count; i++)
			{
				//Make a blank stamp child object
				var child = new GameObject("Dummy");
				child.transform.parent = stamp.transform;

				//Randomize local position within brush radius
				var p = Random.insideUnitCircle;
				child.transform.localPosition = new Vector3(p.x, 0, p.y) * gardener.brushRadius;

				//Randomize local rotation
				var eulerAngles = Vector3.zero;
				if (gardener.maxRandomRotation > 0)
				{
					eulerAngles.y = Random.value * gardener.maxRandomRotation;
					if (gardener.rotationStep > 0)
						eulerAngles.y = Mathf.Round(eulerAngles.y / gardener.rotationStep) * gardener.rotationStep;
				}
				child.transform.localEulerAngles = eulerAngles;

				//Make a stamp dummy and
				GameObject dummy;
				dummy = PrefabUtility.InstantiatePrefab(gardener.SelectedPrefab) as GameObject;
				foreach (var c in dummy.GetComponentsInChildren<Collider>())
					c.enabled = false;
				dummy.transform.parent = child.transform;
				dummy.transform.localPosition = Vector3.zero;
				dummy.transform.localRotation = Quaternion.identity;
			}

			var toDestroy = new HashSet<GameObject>();
			for (var i = 0; i < stamp.transform.childCount; i++)
			{
				var child = stamp.transform.GetChild(i);
				if (toDestroy.Contains(child.gameObject)) continue;

				var bounds = child.GetComponentInChildren<Renderer>().bounds;
				var childVolume = bounds.size.x * bounds.size.y * bounds.size.z;
				for (var x = 0; x < stamp.transform.childCount; x++)
				{
					var check = stamp.transform.GetChild(x);
					if (check.gameObject == child.gameObject) continue;
					if (toDestroy.Contains(check.gameObject)) continue;
					// var b = check.gameObject.GetRendererBounds();
					var b = check.GetComponentInChildren<Renderer>().bounds;
					if (b.Intersects(bounds))
					{
						var overlapVolume = b.size.x * b.size.y * b.size.z;
						var intersection = Intersection(b, bounds);
						var intersectionVolume = intersection.size.x * intersection.size.y * intersection.size.z;
						// Handles.DrawWireCube(intersection.center, intersection.size);
						var maxIntersection = Mathf.Max(intersectionVolume / overlapVolume, intersectionVolume / childVolume);
						// Handles.Label(intersection.center, maxIntersection.ToString());
						if (maxIntersection > gardener.maxIntersectionVolume)
						{
							toDestroy.Add(child.gameObject);
							break;
						}
					}
				}
			}
			foreach (var i in toDestroy)
			{
				DestroyImmediate(i);
			}
		}

		void PerformErase()
		{
			foreach (var g in erase)
				Undo.DestroyObjectImmediate(g);
			erase.Clear();
		}

		void PerformStamp()
		{
			// var removeVariations = ip.SelectedPrefab.GetComponent<Variations>() != null;
			for (var i = 0; i < stamp.transform.childCount; i++)
			{
				var dummy = stamp.transform.GetChild(i);
				if (dummy.gameObject.activeSelf)
				{
					var stampObject = dummy.transform.GetChild(0);
					var prefab = PrefabUtility.GetCorrespondingObjectFromSource(stampObject.gameObject);
					var g = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
					Undo.RegisterCreatedObjectUndo(g, "Stamp");
					// if (removeVariations) DestroyImmediate(g.GetComponent<Variations>());
					g.transform.position = stampObject.position;
					g.transform.rotation = stampObject.rotation;
					g.transform.localScale = stampObject.lossyScale;
					if (gardener.rootTransform != null)
					{
						g.transform.parent = gardener.rootTransform;
						g.isStatic = gardener.rootTransform.gameObject.isStatic;
						g.layer = gardener.rootTransform.gameObject.layer;
					}
				}
			}
			if (gardener.randomizeAfterStamp)
				CreateNewStamp();
		}

		void RotateStamp(Vector2 delta)
		{
			var rotation = Quaternion.AngleAxis(delta.x, Vector3.up);
			foreach (Transform t in stamp.transform)
			{
				t.localPosition = rotation * t.localPosition;
			}
		}

		void AdjustMaxScale(float s)
		{
			for (var i = 0; i < stamp.transform.childCount; i++)
				stamp.transform.GetChild(i).localScale *= s;
		}

	}
}