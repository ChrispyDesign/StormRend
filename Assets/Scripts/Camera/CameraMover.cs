using System.Collections;
using pokoro.BhaVE.Core.Variables;
using StormRend.MapSystems.Tiles;
using StormRend.Units;
using UnityEngine;

namespace StormRend.CameraSystem
{
	[RequireComponent(typeof(CameraInput))]
	public class CameraMover : MonoBehaviour
	{
		//Inspector
		[Header("Movement")]
		[SerializeField] float linearLerp = 0.2f;
		[SerializeField] float linearSpeed = 10f;
		[SerializeField] BoxCollider cameraLimits = null;

		[Header("Edge Panning")]
		[SerializeField] BhaveBool edgePanningOn = null;
		[Tooltip("Pixels"), SerializeField] float edgePanBorderSize = 20f;

		[Header("Drag")]
		[SerializeField] float dragSpeed = 450f;
		[SerializeField] float dragLerp = 0.2f;
		// [SerializeField] float zoomTemperingFactor = 0.5f;

		// [Header("Rotate")]
		// [SerializeField] bool rotateOn = false;
		// [SerializeField] float angularLerp = 0.4f;
		// [SerializeField] float angularSpeed = 100f;

		//Properties
		public bool isInDragMode => input.drag != null;	//If input.drag is valid then that means we're dragging

		//Members
		Camera cam = null;
		CameraInput input = null;
		Vector3 desiredPosition = Vector3.zero;
		float desiredAngle = 0f;

		void Awake()
		{
			cam = MasterCamera.current.camera;
			input = GetComponent<CameraInput>();
		}
		void Start()
		{
			desiredPosition = transform.position;
			desiredAngle = transform.rotation.eulerAngles.y;

			Debug.Assert(edgePanningOn != null, "SOV not found!");
		}
		void Update()
		{
			HandleMoveAndRotate();
			HandleDragMove();
			HandleEdgePanning();
		}

		void LateUpdate() => transform.position = Vector3.Lerp(transform.position, desiredPosition, (input.drag != null) ? dragLerp : linearLerp);

		void HandleMoveAndRotate()
		{
			if (Mathf.Approximately(0, input.xAxis) && Mathf.Approximately(0, input.yAxis)) return;

			//Stop any current move routines
			StopAllCoroutines();

			//Move
			var deltaSpeed = linearSpeed * Time.unscaledDeltaTime;
			desiredPosition += transform.right * input.xAxis * deltaSpeed;
			desiredPosition += transform.forward * input.yAxis * deltaSpeed;

			//Limit to boundary
			desiredPosition = cameraLimits.ClosestPoint(desiredPosition);
		}

		void HandleDragMove()
		{
			if (input.drag == null) return;

			StopAllCoroutines();
			var deltaSpeed = dragSpeed * Time.unscaledDeltaTime;
			desiredPosition += transform.right * -input.drag.Value.x * deltaSpeed;
			desiredPosition += transform.forward * -input.drag.Value.y * deltaSpeed;
			desiredPosition = cameraLimits.ClosestPoint(desiredPosition);
		}

		void HandleEdgePanning()
		{
			if (!edgePanningOn.value) return;

			Vector2Int direction = Vector2Int.zero;

			if (input.mousePosition.x < edgePanBorderSize) direction += Vector2Int.left;
			if (input.mousePosition.x > Screen.width - edgePanBorderSize) direction += Vector2Int.right;
			if (input.mousePosition.y < edgePanBorderSize) direction += Vector2Int.down;
			if (input.mousePosition.y > Screen.height - edgePanBorderSize) direction += Vector2Int.up;

			//Check
			if (direction == Vector2Int.zero) return;

			//Move
			StopAllCoroutines();
			var deltaSpeed = linearSpeed * Time.unscaledDeltaTime;  //Reduce being affected by pause
			desiredPosition += direction.x * transform.right * deltaSpeed;
			desiredPosition += direction.y * transform.forward * deltaSpeed;

			//Limit
			desiredPosition = cameraLimits.ClosestPoint(desiredPosition);
		}

		public void Move(Unit unit, float lerp = 1f) => Move(unit.transform.position, lerp);
		public void Move(Tile tile, float lerp = 1f) => Move(tile.transform.position, lerp);
		public void Move(Vector3 destination, float lerp = 1f)
		{
			StopAllCoroutines();
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