using StormRend.Abilities;
using StormRend.CameraSystem;
using StormRend.MapSystems.Tiles;
using StormRend.Units;
using UnityEngine;

namespace StormRend.Assists
{
    public class CameraFocuser : MonoBehaviour
    {
		//Inspector
		[SerializeField] float lerpTime = 1f;

		//Members
		CameraMover cameraMover = null;

		void Awake() => cameraMover = MasterCamera.current.GetComponent<CameraMover>();

		void Start()
		{
			if (!cameraMover)
			{
				Debug.LogWarning("Camera mover not found! Disabling...");
				enabled = false;
			}
		}

		public void Focus(Tile target) => cameraMover.MoveTo(target, lerpTime);
		public void Focus(Unit target) => cameraMover.MoveTo(target, lerpTime);
		public void Focus(Vector3 target) => cameraMover.MoveTo(target, lerpTime);
		public void Focus(Ability a) => cameraMover.MoveTo(a.lastTargetPos, lerpTime);
	}
}