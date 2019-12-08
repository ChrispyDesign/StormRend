/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

using System.Collections.Generic;
using StormRend.MapSystems;
using UnityEditor;
using UnityEngine;

namespace StormRend.Editors
{
	[CustomEditor(typeof(PropPainter))]
	public partial class PropPainterEditor : SmartEditor
	{
		Texture2D[] paletteImages;
		GameObject stamp;

		List<GameObject> erase = new List<GameObject>();
		Vector3 worldCursor;
		PropPainter pp;
		List<Bounds> overlaps = new List<Bounds>();
		List<GameObject> overlappedGameObjects = new List<GameObject>();

		void OnEnable()
		{
			stamp = new GameObject("Stamp");
			stamp.hideFlags = HideFlags.HideAndDontSave;
			pp = target as PropPainter;

			//Prevents a blank stamp on startup
			CreateNewStamp();
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
			var stampChildCount = Mathf.Min(1000, (Mathf.PI * Mathf.Pow(pp.brushRadius, 2)) / (1f / pp.brushDensity));

			//Make stamp children (A stamp child is a prefab instance of a the current selected prefab)
			for (var i = 0; i < stampChildCount; i++)
			{
				//Make a blank stamp child object and parent to stamp
				var child = new GameObject("Dummy");
				child.transform.parent = stamp.transform;

				//Randomize local position within brush radius
				var pos = Random.insideUnitCircle;
				child.transform.localPosition = new Vector3(pos.x, 0, pos.y) * pp.brushRadius;

				//Randomize local rotation
				var rotY = Vector3.zero;
				if (pp.maxRandomRotation > 0) rotY.y = Random.value * pp.maxRandomRotation;
				child.transform.localEulerAngles = rotY;

				//Instantiate a copy of selected prefab and parent to the child
				GameObject copy;
				copy = PrefabUtility.InstantiatePrefab(pp.SelectedPrefab) as GameObject;
				foreach (var c in copy.GetComponentsInChildren<Collider>())
					c.enabled = false;
				copy.transform.parent = child.transform;
				copy.transform.localPosition = Vector3.zero;
				copy.transform.localRotation = Quaternion.identity;
			}

			//Remove invalid stamp children
			var toDestroy = new HashSet<GameObject>();
			for (var i = 0; i < stamp.transform.childCount; i++)
			{
				var child = stamp.transform.GetChild(i);

				//Skip duplicates
				if (toDestroy.Contains(child.gameObject)) continue;

				//Get stamp child renderer bounds
				var firstChildBounds = child.GetComponentInChildren<Renderer>().bounds;
				var childVolume = firstChildBounds.size.x * firstChildBounds.size.y * firstChildBounds.size.z;

				//Loop through the other stamp children
				for (var j = 0; j < stamp.transform.childCount; j++)
				{
					var check = stamp.transform.GetChild(j);
					if (check.gameObject == child.gameObject) continue;		//Skip same
					if (toDestroy.Contains(check.gameObject)) continue;		//Skip duplicates
					// var b = check.gameObject.GetRendererBounds();
					var secondChildBounds = check.GetComponentInChildren<Renderer>().bounds;
					if (secondChildBounds.Intersects(firstChildBounds))
					{
						var overlapVolume = secondChildBounds.size.x * secondChildBounds.size.y * secondChildBounds.size.z;
						var intersection = Intersection(secondChildBounds, firstChildBounds);
						var intersectionVolume = intersection.size.x * intersection.size.y * intersection.size.z;
						// Handles.DrawWireCube(intersection.center, intersection.size);
						var maxIntersection = Mathf.Max(intersectionVolume / overlapVolume, intersectionVolume / childVolume);
						// Handles.Label(intersection.center, maxIntersection.ToString());
						if (maxIntersection > pp.maxDensity)
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
			for (var i = 0; i < stamp.transform.childCount; i++)
			{
				var dummy = stamp.transform.GetChild(i);
				if (dummy.gameObject.activeSelf)
				{
					var stampObject = dummy.transform.GetChild(0);
					var prefab = PrefabUtility.GetCorrespondingObjectFromSource(stampObject.gameObject);
					var g = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
					Undo.RegisterCreatedObjectUndo(g, "Stamp");
					g.transform.position = stampObject.position;
					g.transform.rotation = stampObject.rotation;
					g.transform.localScale = stampObject.lossyScale;
					if (pp.rootTransform != null)
					{
						g.transform.parent = pp.rootTransform;
						g.isStatic = pp.rootTransform.gameObject.isStatic;
						g.layer = pp.rootTransform.gameObject.layer;
					}
				}
			}

			if (pp.randomizeEachStamp) CreateNewStamp();
		}

		void RotateStamp(Vector2 delta)
		{
			var rotation = Quaternion.AngleAxis(delta.y, Vector3.up);
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