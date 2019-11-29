using System;
using System.Collections;
using StormRend.MapSystems.Tiles;
using StormRend.Units;
using UnityEngine;

namespace StormRend.CameraSystem
{
	/// <summary>
	/// camera movement class, responsible for the translation of the camera
	/// </summary>
	[RequireComponent(typeof(CameraInput))]
	public class xCameraMover : MonoBehaviour
	{
		//Inspector
		[SerializeField] BoxCollider cameraBounds = null;
		[SerializeField] Transform root = null;
		[SerializeField] float moveSpeed = 10;
		[SerializeField] float lerp = 0.1f;

		//Members
		CameraInput cin;
		private Vector3 desiredPosition;

		#region Core
		void Awake()
		{
			cin = GetComponent<CameraInput>();
		}

		void Start() => desiredPosition = root.transform.position;

		void Update() => MoveCameraByInput();

		void LateUpdate() => root.position = Vector3.Lerp(root.position, desiredPosition, lerp);

		void MoveCameraByInput()
		{
			//If there's input then override any current moving coroutines
			var moveAxis = new Vector3(cin.xAxis, 0, cin.yAxis);
			if (!moveAxis.Equals(Vector3.zero))
				MoveBy(moveAxis);
		}
		#endregion
		
		public void MoveBy(Vector3 axis)
		{
			//Override any current lerps
			StopAllCoroutines();

			//Set desired position to be lerped to
			desiredPosition += axis.z * root.forward * moveSpeed * Time.unscaledDeltaTime;
			desiredPosition += axis.y * root.up * moveSpeed * Time.unscaledDeltaTime;
			desiredPosition += axis.x * root.right * moveSpeed * Time.unscaledDeltaTime;
			
			RestrictToBounds(ref desiredPosition);
		}

		/// <summary>
		/// Camera lerp override for units
		/// </summary>
		public void MoveTo(Unit unit, float smoothTime = 0.3f) => MoveTo(unit.transform.position, smoothTime);

		/// <summary>
		/// Camera lerp override for tiles
		/// </summary>
		public void MoveTo(Tile tile, float smoothTime = 0.3f) => MoveTo(tile.transform.position, smoothTime);

		/// <summary>
		/// Camera lerp
		/// </summary>
		public void MoveTo(Vector3 destination, float smoothTime = 0.3f)
		{
			RestrictToBounds(ref destination);

			//Override any current lerps
			StopAllCoroutines();

			//Clamp within bounds
			RestrictToBounds(ref destination);

			//Start move
			StartCoroutine(LerpTo(destination, smoothTime));
		}

		/// <summary>
		/// Camera lerp coroutine. Will override and set desiredPosition
		/// </summary>
		IEnumerator LerpTo(Vector3 destination, float time = 0.3f)
		{
			float timer = 0;

			while (timer < time)
			{
				// get lerp percentage & increment timer
				float t = timer / time;
				timer += Time.unscaledDeltaTime;

				//Set root position and also override desired position to nullify LateUpdate's lerp logic
				root.position = desiredPosition = Vector3.Lerp(root.position, destination, t);
				yield return null;
			}
		}

		/// <summary>
		/// Limit camera within to boundary object
		/// </summary>
		void RestrictToBounds(ref Vector3 position) => position = cameraBounds.ClosestPoint(position);
	}
}