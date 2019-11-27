using StormRend.CameraSystem;
using StormRend.MapSystems.Tiles;
using UnityEngine;
using UnityEngine.UI;

namespace StormRend.Units
{
	public class HealthBar : MonoBehaviour
	{
		public Image bar;
		Camera cam;
		AnimateUnit au;

		void Awake()
		{
			cam = MasterCamera.current.camera;
			au = GetComponentInParent<AnimateUnit>();

			Debug.Assert(au, "Animate unit not found!");
		}

		//Register callbacks
		void OnEnable()
		{
			au.onHeal.AddListener(OnHealthChange);
			au.onTakeDamage.AddListener(OnHealthChange);
			au.onMoved.AddListener(UpdateFacing);

			UpdateFacing();
		}
		void OnDisable()
		{
			au.onHeal.RemoveAllListeners();
			au.onTakeDamage.RemoveAllListeners();
			au.onMoved.RemoveAllListeners();
		}

		//Init bar at start
		void Start() => bar.fillAmount = (float)au.HP / au.maxHP;

		//Always face toward the camera
		void UpdateFacing(Tile t = null) => transform.rotation = Quaternion.AngleAxis(MasterCamera.current.transform.rotation.eulerAngles.y - 180f, Vector3.up);

		//Callbacks
		public void OnHealthChange(HealthData damageData) => OnHealthChange();
		public void OnHealthChange() => bar.fillAmount = (float)au.HP / au.maxHP;
	}
}