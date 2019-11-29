using System.Collections;
using StormRend.MapSystems.Tiles;
using StormRend.Units;
using UnityEngine;

namespace StormRend.CameraSystem
{
	[RequireComponent(typeof(CameraInput))]
	public class CameraMover : MonoBehaviour
	{
		//Inspector
		[SerializeField] BoxCollider cameraLimits = null;
		[SerializeField] float linearLerp = 0.2f;
		[SerializeField] float linearSpeed = 10f;

		[SerializeField] bool rotateOn = false;
		[SerializeField] float angularLerp = 0.4f;
		[SerializeField] float angularSpeed = 100f;

		//Members
		CameraInput input = null;
		Vector3 desiredPosition = Vector3.zero;
		float desiredAngle = 0f;

		void Awake() => input = GetComponent<CameraInput>();
		void Start()
		{
			desiredPosition = transform.position;
			desiredAngle = transform.rotation.eulerAngles.y;
		}
		void Update()
		{
			HandleMoveAndRotate(new Vector3(input.xAxis, 0, input.yAxis));
		}

		void LateUpdate() => transform.position = Vector3.Lerp(transform.position, desiredPosition, linearLerp);

		void HandleMoveAndRotate(Vector3 moveAxis)
		{
			if (moveAxis.Equals(Vector3.zero)) return;

			//Stop any current move routines
			StopAllCoroutines();

			//Move
			var deltaSpeed = linearSpeed * Time.unscaledDeltaTime;
			desiredPosition += transform.right * input.xAxis * deltaSpeed;
			desiredPosition += transform.forward * input.yAxis * deltaSpeed;

			//Rotate

			//Limit to boundary
			desiredPosition = cameraLimits.ClosestPoint(desiredPosition);
		}

		public void Move(Unit unit, float lerp = 1f) => Move(unit.transform.position, lerp);
		public void Move(Tile tile , float lerp = 1f) => Move(tile.transform.position, lerp);
		public void Move(Vector3 destination, float lerp = 1f)
		{
			StopAllCoroutines();
			// StartCoroutine(Lerp(cameraLimits.ClosestPoint(destination), lerp));
			StartCoroutine(Lerp(cameraLimits.ClosestPoint(destination), lerp));
		}

		IEnumerator Lerp(Vector3 destination, float time = 1f)
		{
			float timer = 0;
			while (timer < time)
			{
				// get lerp percentage & increment timer
				float t = timer / time;
				timer += Time.unscaledDeltaTime;

				//Set root position and also override desired position to nullify LateUpdate's lerp logic
				transform.position = desiredPosition = Vector3.Lerp(transform.position, destination, t);
				yield return null;
			}
		}
	}
}